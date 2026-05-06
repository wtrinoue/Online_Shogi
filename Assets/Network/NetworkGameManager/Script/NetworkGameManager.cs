using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Fusion;

public class NetworkGameManager : NetworkBehaviour, IGameManager
{
    // =========================
    // Network State（唯一の状態）
    // =========================
    [Networked, Capacity(81)]
    public NetworkArray<NetworkPieceData> PieceBoard => default;

    [Networked, Capacity(81)]
    public NetworkArray<NetworkCellState> CellBoard => default;

    [Networked, Capacity(20)]
    public NetworkArray<NetworkPieceData> SenteHandPiece => default;

    [Networked, Capacity(20)]
    public NetworkArray<NetworkCellState> SenteHandCell => default;

    [Networked, Capacity(20)]
    public NetworkArray<NetworkPieceData> GoteHandPiece => default;

    [Networked, Capacity(20)]
    public NetworkArray<NetworkCellState> GoteHandCell => default;
    // =========================
    // NetworkWaitStateにおける次のStateへの通過用
    // =========================

    [Networked]
    public int MoveSignal { get; set; }

    [Networked]
    public Team LastMovedTeam { get; set; }

    [Networked]
    public PlayerRef SentePlayer { get; set; }

    [Networked]
    public PlayerRef GotePlayer { get; set; }
    [Networked]
    public int RenderSignal { get; set; }

    // =========================
    // Local View State
    // =========================
    public BoardSetup boardSetup;
    public PieceFactory pieceFactory;

    private Vector2Int selectedBoardPiecePos;
    private Vector2Int selectedSenteHandPos;
    private Vector2Int selectedGoteHandPos;
    private Piece selectedPiece;
    // =========================
    // 連携用
    // =========================
    public StateMachine stateMachine;
    public GameViewer gameViewer;
    public override void Spawned()
    {
        IGameManager gm = this.GetComponent<IGameManager>();
        stateMachine = FindObjectOfType<StateMachine>();
        gameViewer = FindObjectOfType<GameViewer>();
        stateMachine.SetGameManager(gm);
        gameViewer.SetGameManager(gm);
    }

    // =========================
    // RPC
    // =========================
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_MovePieceFromBoard(int fi, int ti)
    {
        var piece = PieceBoard[fi];

        PieceBoard.Set(ti, piece);
        PieceBoard.Set(fi, default);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_MovePieceFromSenteHand(int fromIndex, int toIndex)
    {
        var piece = SenteHandPiece[fromIndex];

        RPC_SenteRemovePiece(fromIndex);

        PieceBoard.Set(toIndex, piece);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_MovePieceFromGoteHand(int fromIndex, int toIndex)
    {
        var piece = GoteHandPiece[fromIndex];

        RPC_GoteRemovePiece(fromIndex);

        PieceBoard.Set(toIndex, piece);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetBoardCells(int[] indices, CellState state)
    {
        Debug.Log("RPC_SetBoardCells");
        foreach (var i in indices)
        {
            var c = CellBoard[i];
            c.State = state;

            CellBoard.Set(i, c);
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetSenteHandCells(int[] indices, CellState state)
    {
        foreach (var i in indices)
        {
            var c = SenteHandCell[i];
            c.State = state;

            SenteHandCell.Set(i, c);
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetGoteHandCells(int[] indices, CellState state)
    {
        foreach (var i in indices)
        {
            var c = GoteHandCell[i];
            c.State = state;

            GoteHandCell.Set(i, c);
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SenteAddPiece(NetworkPieceData piece)
    {
        for (int i = 0; i < 20; i++)
        {
            if (SenteHandPiece[i].Type == 0)
            {
                SenteHandPiece.Set(i, piece);
                return;
            }
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SenteRemovePiece(int index)
    {
        for (int i = index; i < 19; i++)
        {
            SenteHandPiece.Set(i, SenteHandPiece[i + 1]);
        }

        SenteHandPiece.Set(19, default);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_GoteAddPiece(NetworkPieceData piece)
    {
        for (int i = 0; i < 20; i++)
        {
            if (GoteHandPiece[i].Type == 0)
            {
                GoteHandPiece.Set(i, piece);
                return;
            }
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_GoteRemovePiece(int index)
    {
        for (int i = index; i < 19; i++)
        {
            GoteHandPiece.Set(i, GoteHandPiece[i + 1]);
        }

        GoteHandPiece.Set(19, default);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_PromotePiece(int index)
    {
        var p = PieceBoard[index];
        p.IsPromoted = true;
        PieceBoard.Set(index, p);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SignalMove(Team movedTeam)
    {
        Debug.Log("MoveSignal++");
        MoveSignal++;
        LastMovedTeam = movedTeam;
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RenderSignalMove()
    {
        RenderSignal++;
    }

    // =========================
    // Init
    // =========================
    public void Init()
    {
        if (!Object.HasStateAuthority) return;
        Debug.Log("NetworkGameManagerでInitしました");
        InitializePiece();
        InitializeCell();
        DebugPrintPieceBoard();
        var board = GetPieceBoard();

        int nullCount = 0;

        for (int y = 0; y < 9; y++)
        for (int x = 0; x < 9; x++)
        {
            if (board[x, y] == null)
                nullCount++;
        }

        Debug.Log("Null pieces: " + nullCount);
    }

    public void InitializePiece()
    {
        for (int i = 0; i < 81; i++)
        {
            PieceBoard.Set(i, default);
        }

        foreach (var p in boardSetup.placements)
        {
            int i = ToIndex(new Vector2Int(p.x, p.y));

            PieceBoard.Set(i, new NetworkPieceData
            {
                Team = p.pieceData.team,
                Type = p.pieceData.type,
                IsPromoted = false,
                IsHanded = false
            });
        }
    }

    public void InitializeCell()
    {
        for (int i = 0; i < 81; i++)
        {
            CellBoard.Set(i, new NetworkCellState
            {
                State = CellState.Normal
            });
        }

        for (int i = 0; i < 20; i++)
        {
            SenteHandCell.Set(i, new NetworkCellState { State = CellState.Normal });
            GoteHandCell.Set(i, new NetworkCellState { State = CellState.Normal });
        }
    }

    // =========================
    // Promote
    // =========================
    public void PromotePiece(Vector2Int pos)
    {
        int i = ToIndex(pos);

        RPC_PromotePiece(i);
    }

    public bool IsPromotable(Vector2Int pos)
    {
        var p = PieceBoard[ToIndex(pos)];
        if (p.Type == 0) return false;
        if (p.IsHanded) return false;

        return p.Team == Team.Sente ? pos.y >= 6 : pos.y <= 2;
    }

    // =========================
    // Move Board
    // =========================
    public Piece MovePieceFromBoard(Vector2Int from, Vector2Int to)
    {
        var target = GetBoardPiece(to);

        int fi = ToIndex(from);
        int ti = ToIndex(to);

        RPC_MovePieceFromBoard(fi, ti);

        return target;
    }

    // =========================
    // Hand Move
    // =========================
    public Piece MovePieceFromSenteHand(Vector2Int from, Vector2Int to)
    {
        int fi = ToHandIndex(from);
        int ti = ToIndex(to);

        var piece = SenteHandPiece[fi];
        if (piece.Type == 0)
            return null;

        var targetPiece = ToLocal(PieceBoard[ti]);

        RPC_MovePieceFromSenteHand(fi, ti);

        return targetPiece;
    }

    public Piece MovePieceFromGoteHand(Vector2Int from, Vector2Int to)
    {
        int fi = ToHandIndex(from);
        int ti = ToIndex(to);

        var piece = GoteHandPiece[fi];
        if (piece.Type == 0)
            return null;

        var targetPiece = ToLocal(PieceBoard[ti]);

        RPC_MovePieceFromGoteHand(fi, ti);

        return targetPiece;
    }

    // =========================
    // Add / Hand管理（可変長扱い）
    // =========================
    public void AddToHand(Piece piece)
    {
        if (piece == null) return;

        // =========================
        // 王を取ったら勝敗
        // =========================
        if (piece.data.type == PieceType.Ou)
        {
            ChangeGameState(piece.data.team == Team.Sente
                ? GameState.GoteWin
                : GameState.SenteWin);
            return;
        }

        // =========================
        // 重要：取った駒は「相手の駒」に変換する
        // =========================
        Team targetTeam = (piece.data.team == Team.Sente)
            ? Team.Gote
            : Team.Sente;

        var converted = pieceFactory.GetPiece(
            targetTeam,
            piece.data.type,
            false,
            true
        );

        var data = ToNetwork(converted);

        // =========================
        // 持ち駒へ追加
        // =========================
        if (targetTeam == Team.Sente)
        {
            for (int i = 0; i < 20; i++)
            {
                if (SenteHandPiece[i].Type == 0)
                {
                    RPC_SenteAddPiece(data);
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < 20; i++)
            {
                if (GoteHandPiece[i].Type == 0)
                {
                    RPC_GoteAddPiece(data);
                    return;
                }
            }
        }
    }

    public void TestAddToHand()
    {
        for (int x = 0; x < 9; x++)
        {
            AddToHand(GetBoardPiece(new Vector2Int(x, 0)));
            AddToHand(GetBoardPiece(new Vector2Int(x, 8)));
        }
    }

    // =========================
    // Cells
    // =========================
    public void ClearCells()
    {
        // =========================
        // 盤面（9x9）
        // =========================
        var boardList = new List<int>();

        for (int y = 0; y < 9; y++)
        for (int x = 0; x < 9; x++)
            boardList.Add(ToIndex(new Vector2Int(x, y)));

        RPC_SetBoardCells(boardList.ToArray(), CellState.Normal);

        // =========================
        // 先手持ち駒（2x10想定）
        // =========================
        var senteList = new List<int>();

        for (int y = 0; y < 10; y++)
        for (int x = 0; x < 2; x++)
            senteList.Add(ToHandIndex(new Vector2Int(x, y)));

        RPC_SetSenteHandCells(senteList.ToArray(), CellState.Normal);

        // =========================
        // 後手持ち駒（2x10想定）
        // =========================
        var goteList = new List<int>();

        for (int y = 0; y < 10; y++)
        for (int x = 0; x < 2; x++)
            goteList.Add(ToHandIndex(new Vector2Int(x, y)));

        RPC_SetGoteHandCells(goteList.ToArray(), CellState.Normal);
        RPC_RenderSignalMove();
    }

    public void ChangeBoardCells(List<Vector2Int> posList)
    {
        var indices = new int[posList.Count];
        for (int i = 0; i < posList.Count; i++)
            indices[i] = ToIndex(posList[i]);

        RPC_SetBoardCells(indices, CellState.Placeable);
        RPC_RenderSignalMove();
    }

    public void ChangeBoardCellSelected(Vector2Int pos)
    {
        RPC_SetBoardCells(new[] { ToIndex(pos) }, CellState.Selected);
        RPC_RenderSignalMove();
    }

    public void ChangeSenteHandCellSelected(Vector2Int pos)
    {
        RPC_SetSenteHandCells(new[] { ToHandIndex(pos) }, CellState.Selected);
        RPC_RenderSignalMove();
    }

    public void ChangeGoteHandCellSelected(Vector2Int pos)
    {
        RPC_SetGoteHandCells(new[] { ToHandIndex(pos) }, CellState.Selected);
        RPC_RenderSignalMove();
    }

    // =========================
    // Moveable（簡易）
    // =========================
    public List<Vector2Int> GetHandPieceMovablePositions()
    {
        List<Vector2Int> result = new();

        for (int i = 0; i < 81; i++)
        {
            if (PieceBoard[i].Type == 0)
                result.Add(new Vector2Int(i % 9, i / 9));
        }

        return result;
    }

    public List<Vector2Int> GetBoardPieceMovablePositions(Vector2Int pos)
    {
        int fromIndex = ToIndex(pos);
        var pieceData = PieceBoard[fromIndex];

        if (pieceData.Type == 0)
            return new List<Vector2Int>();

        Piece piece = ToLocal(pieceData);
        MovePattern movePattern = piece.GetMovePattern();

        Vector2Int[] direction = movePattern.direction;
        Vector2Int[] position = movePattern.position;

        List<Vector2Int> result = new List<Vector2Int>();

        // =========================
        // 方向（伸びる移動）
        // =========================
        foreach (var vec in direction)
        {
            for (int i = 1; i < 9; i++)
            {
                Vector2Int targetPos = pos + vec * i;

                if (!IsInsideBoard(targetPos))
                    continue;

                int targetIndex = ToIndex(targetPos);
                var targetData = PieceBoard[targetIndex];

                // 空マス
                if (targetData.Type == 0)
                {
                    result.Add(targetPos);
                    continue;
                }

                // 同じチーム
                if (targetData.Team == pieceData.Team)
                {
                    break;
                }
                // 敵駒
                else
                {
                    result.Add(targetPos);
                    break;
                }
            }
        }

        // =========================
        // 固定移動
        // =========================
        foreach (var vec in position)
        {
            Vector2Int targetPos = pos + vec;

            if (!IsInsideBoard(targetPos))
                continue;

            int targetIndex = ToIndex(targetPos);
            var targetData = PieceBoard[targetIndex];

            // 空
            if (targetData.Type == 0)
            {
                result.Add(targetPos);
                continue;
            }

            // 同じチームなら不可
            if (targetData.Team == pieceData.Team)
            {
                continue;
            }

            // 敵なら取れる
            result.Add(targetPos);
        }

        return result;
    }

    // =========================
    // Getters（全部Networkから生成）
    // =========================
    public Piece GetBoardPiece(Vector2Int pos) => ToLocal(PieceBoard[ToIndex(pos)]);
    public Piece GetSenteHandPiece(Vector2Int pos) => ToLocal(SenteHandPiece[ToHandIndex(pos)]);
    public Piece GetGoteHandPiece(Vector2Int pos) => ToLocal(GoteHandPiece[ToHandIndex(pos)]);
    public Cell GetBoardCell(Vector2Int pos) => new Cell { state = CellBoard[ToIndex(pos)].State };

    public Piece[,] GetPieceBoard()
    {
        var b = new Piece[9, 9];
        for (int i = 0; i < 81; i++)
            b[i % 9, i / 9] = ToLocal(PieceBoard[i]);
        return b;
    }

    public Cell[,] GetCellBoard()
    {
        var b = new Cell[9, 9];
        for (int i = 0; i < 81; i++)
            b[i % 9, i / 9] = new Cell { state = CellBoard[i].State };
        return b;
    }

    public List<Piece> GetSenteHandPieces()
    {
        var list = new List<Piece>();
        for (int i = 0; i < 20; i++)
            if (SenteHandPiece[i].Type != 0)
                list.Add(ToLocal(SenteHandPiece[i]));
        Debug.Log($"SenteHandList:{list.Count}");
        return list;
    }

    public Cell[,] GetSenteHandCells()
    {
        var b = new Cell[2, 10];
        for (int i = 0; i < 20; i++)
            b[i / 10, i % 10] = new Cell { state = SenteHandCell[i].State };
        return b;
    }

    public List<Piece> GetGoteHandPieces()
    {
        var list = new List<Piece>();
        for (int i = 0; i < 20; i++)
            if (GoteHandPiece[i].Type != 0)
                list.Add(ToLocal(GoteHandPiece[i]));
        Debug.Log($"GoteHandList:{list.Count}");
        return list;
    }

    public Cell[,] GetGoteHandCells()
    {
        var b = new Cell[2, 10];
        for (int i = 0; i < 20; i++)
            b[i / 10, i % 10] = new Cell { state = GoteHandCell[i].State };
        return b;
    }

    // =========================
    // Selection
    // =========================
    public void SetSelectedBoardPiecePosition(Vector2Int pos)
    {
        selectedBoardPiecePos = pos;
        selectedPiece = GetBoardPiece(pos);
    }

    public void SetSelectedSenteHandPiecePosition(Vector2Int pos)
    {
        selectedSenteHandPos = pos;
        selectedPiece = GetSenteHandPiece(pos);
    }

    public void SetSelectedGoteHandPiecePosition(Vector2Int pos)
    {
        selectedGoteHandPos = pos;
        selectedPiece = GetGoteHandPiece(pos);
    }

    public Vector2Int GetSelectedBoardPiecePosition() => selectedBoardPiecePos;
    public Vector2Int GetSelectedSenteHandPiecePosition() => selectedSenteHandPos;
    public Vector2Int GetSelectedGoteHandPiecePosition() => selectedGoteHandPos;
    public Piece GetSelectedPiece() => selectedPiece;

    // =========================
    // Utils
    // =========================
    public bool IsInsideBoard(Vector2Int p) => p.x >= 0 && p.x < 9 && p.y >= 0 && p.y < 9;
    public bool IsPlaceableOnBoard(Vector2Int pos) => CellBoard[ToIndex(pos)].State == CellState.Placeable;

    private int ToIndex(Vector2Int p) => p.x + p.y * 9;
    private int ToHandIndex(Vector2Int p) => p.x * 10 + p.y;

    private Piece ToLocal(NetworkPieceData d)
    {
        // Debug.Log($"Team:{d.Team} Type:{d.Type} {(int)d.Type}");
        if (d.Type == 0) return null;
        var data = ScriptableObject.CreateInstance<PieceData>();
        data.team = d.Team;
        data.type = d.Type;
        return pieceFactory.GetPiece(data.team, data.type, d.IsPromoted, d.IsHanded);
    }

    private NetworkPieceData ToNetwork(Piece p)
    {
        return new NetworkPieceData
        {
            Team = p.data.team,
            Type = p.data.type,
            IsPromoted = p.isPromoted,
            IsHanded = p.isHanded
        };
    }

    // =========================
    // Debug / Game Over
    // =========================
    public void DebugPrintPieceBoard()
    {
        Debug.Log("===== PieceBoard =====");

        for (int y = 0; y < 9; y++)
        {
            string line = "";

            for (int x = 0; x < 9; x++)
            {
                var p = PieceBoard[ToIndex(new Vector2Int(x, y))];

                if (p.Type == 0)
                {
                    line += " . ";
                }
                else
                {
                    string t = p.Team == Team.Sente ? "S" : "G";
                    string pr = p.IsPromoted ? "+" : "";
                    line += $"{t}{(int)p.Type}{pr} ";
                }
            }

            Debug.Log(line);
        }
    }
    public void DebugPrintCellBoard() { }

    private GameState gameState = GameState.Playing;
    private void ChangeGameState(GameState newState)
    {
        gameState = newState;
    }
    public bool IsGameOver(out Team winner)
    {
        switch (gameState)
        {
            case GameState.Playing:
                winner = Team.Sente; // ダミーの値
                return false;
            case GameState.SenteWin:
                winner = Team.Sente;
                return true;
            case GameState.GoteWin:
                winner = Team.Gote;
                return true;
            default:
                winner = Team.Sente; // ダミーの値
                return false;
        }
    }
    // =========================
    // NetworkGameManager専用
    // =========================

    // 以下State更新用
    public void SignalMove(Team movedTeam)
    {
        RPC_SignalMove(movedTeam);
    }

    public int GetMoveSignal()
    {
        return MoveSignal;
    }

    public Team GetLastMovedTeam()
    {
        return LastMovedTeam;
    }

    public void ChangeIsMovedTo(bool isMoved){
    }
    public bool GetIsMoved(){
        return false;
    }
    // 以下描画更新用
    public void RenderSignalMove()
    {
        RPC_RenderSignalMove();
    }
    public int GetRenderSignal()
    {
        return RenderSignal;
    }
}

public struct NetworkPieceData : INetworkStruct
{
    public Team Team;
    public PieceType Type;
    public bool IsPromoted;
    public bool IsHanded;
}

public struct NetworkCellState : INetworkStruct
{
    public CellState State;
}
