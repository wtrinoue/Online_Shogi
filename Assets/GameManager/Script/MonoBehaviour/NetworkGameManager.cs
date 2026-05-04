using System.Collections.Generic;
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
    // Local View State
    // =========================
    public BoardSetup boardSetup;
    public PieceFactory pieceFactory;

    private Vector2Int selectedBoardPiecePos;
    private Vector2Int selectedSenteHandPos;
    private Vector2Int selectedGoteHandPos;
    private Piece selectedPiece;

    // =========================
    // Init
    // =========================
    public void Init()
    {
        if (!Object.HasStateAuthority) return;
        InitializePiece();
        InitializeCell();
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
        var p = PieceBoard[i];
        p.IsPromoted = true;
        PieceBoard.Set(i, p);
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
        int fi = ToIndex(from);
        int ti = ToIndex(to);

        var piece = PieceBoard[fi];
        var target = PieceBoard[ti];

        PieceBoard.Set(ti, piece);
        PieceBoard.Set(fi, default);

        return ToLocal(target);
    }

    // =========================
    // Hand Move
    // =========================
    public Piece MovePieceFromSenteHand(Vector2Int from, Vector2Int to)
    {
        int i = ToHandIndex(from);
        var piece = SenteHandPiece[i];

        SenteHandPiece.Set(i, default);
        PieceBoard.Set(ToIndex(to), piece);

        return ToLocal(piece);
    }

    public Piece MovePieceFromGoteHand(Vector2Int from, Vector2Int to)
    {
        int i = ToHandIndex(from);
        var piece = GoteHandPiece[i];

        GoteHandPiece.Set(i, default);
        PieceBoard.Set(ToIndex(to), piece);

        return ToLocal(piece);
    }

    // =========================
    // Add / Hand管理（可変長扱い）
    // =========================
    public void AddToHand(Piece piece)
    {
        if (piece == null) return;

        var data = ToNetwork(piece);

        if (piece.data.team == Team.Sente)
        {
            for (int i = 0; i < 20; i++)
            {
                if (SenteHandPiece[i].Type == 0)
                {
                    SenteHandPiece.Set(i, data);
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
                    GoteHandPiece.Set(i, data);
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
        for (int i = 0; i < 81; i++)
        {
            var c = CellBoard[i];
            c.State = CellState.Normal;
            CellBoard.Set(i, c);
        }
    }

    public void ChangeBoardCells(List<Vector2Int> posList)
    {
        foreach (var p in posList)
        {
            int i = ToIndex(p);
            var c = CellBoard[i];
            c.State = CellState.Placeable;
            CellBoard.Set(i, c);
        }
    }

    public void ChangeBoardCellSelected(Vector2Int pos)
    {
        int i = ToIndex(pos);
        var c = CellBoard[i];
        c.State = CellState.Selected;
        CellBoard.Set(i, c);
    }

    public void ChangeSenteHandCellSelected(Vector2Int pos)
    {
        int i = ToHandIndex(pos);
        var c = SenteHandCell[i];
        c.State = CellState.Selected;
        SenteHandCell.Set(i, c);
    }

    public void ChangeGoteHandCellSelected(Vector2Int pos)
    {
        int i = ToHandIndex(pos);
        var c = GoteHandCell[i];
        c.State = CellState.Selected;
        GoteHandCell.Set(i, c);
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
        return new List<Vector2Int>(); // 既存ロジック流用想定
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
        if (d.Type == 0) return null;
        return new Piece(new PieceData { team = d.Team, type = d.Type }, d.IsPromoted);
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
    public void DebugPrintPieceBoard() { }
    public void DebugPrintCellBoard() { }

    public bool IsGameOver(out Team winner)
    {
        winner = Team.Sente;
        return false;
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