using System.Collections.Generic;
using UnityEngine;
/*
GameManagerの責務について
・将棋の駒を操作すること（ゲーム全体の制御はしない）
・現状を把握するための情報を渡せること
・StateMachineで将棋のルールがわかるような最小単位のメソッドをもつこと
*/
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardSetup boardSetup;
    public PieceFactory pieceFactory;
    // ★ ここを配列に変更
    public Piece[,] pieceBoard = new Piece[9, 9];
    public Cell[,] cellBoard = new Cell[9, 9];

    public List<Piece> senteHandPieces = new List<Piece>();
    public Cell[,] senteHandCells = new Cell[2, 10];
    public List<Piece> goteHandPieces = new List<Piece>();
    public Cell[,] goteHandCells = new Cell[2, 10];


    void Awake()
    {
        Debug.Log("Awake called");
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializePiece();
        InitializeCell();
        DebugPrintCellBoard();
        DebugPrintPieceBoard();
    }
    // -------------------------
    // 取得
    // -------------------------
    public Piece GetBoardPiece(Vector2Int pos)
    {
        return pieceBoard[pos.x, pos.y];
    }
    public Piece GetSenteHandPiece(int index)
    {
        return senteHandPieces[index];
    }
    public Piece GetGoteHandPiece(int index)
    {
        return goteHandPieces[index];
    }
    // -------------------------
    // 初期化
    // -------------------------
    public void InitializePiece()
    {
        foreach (var piecePlacement in boardSetup.placements)
        {
            int x = piecePlacement.x;
            int y = piecePlacement.y;

            PieceData pieceData = piecePlacement.pieceData;
            Piece piece = new Piece(pieceData, false);

            pieceBoard[x, y] = piece;
        }
    }

    public void InitializeCell()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                cellBoard[x, y] = new Cell();
            }
        }
        for (int y = 0; y < 10; y++)
        {
            for(int x = 0; x < 2; x++)
            {
                senteHandCells[x, y] = new Cell();
                goteHandCells[x, y] = new Cell();
            }
        }
    }

    // -------------------------
    // 成り
    // -------------------------
    public void PromotePiece(Vector2Int pos)
    {
        pieceBoard[pos.x, pos.y].Promote();
    }

    public bool IsPromotable(Vector2Int pos)
    {
        Piece piece = pieceBoard[pos.x, pos.y];
        
        if (piece == null) return false;
        if (piece.isHanded) return false;

        Team team = piece.data.team;

        if (team == Team.Sente)
        {
            if (pos.y >= 6) return true;
        }
        else
        {
            if (pos.y <= 2) return true;
        }

        return false;
    }

    // -------------------------
    // 移動
    // -------------------------
    public Piece MovePieceFromBoard(Vector2Int fromPos, Vector2Int toPos)
    {
        Piece piece = pieceBoard[fromPos.x, fromPos.y];
        if (piece == null) return null;

        Piece targetPiece = pieceBoard[toPos.x, toPos.y];

        pieceBoard[toPos.x, toPos.y] = piece;
        pieceBoard[fromPos.x, fromPos.y] = null;

        return targetPiece;
    }

    public Piece MovePieceFromSenteHand(int fromIndex, Vector2Int toPos)
    {
        Piece piece = senteHandPieces[fromIndex];
        if (piece == null) return null;

        Piece targetPiece = pieceBoard[toPos.x, toPos.y];

        pieceBoard[toPos.x, toPos.y] = piece;

        return targetPiece;
    }

    public Piece MovePieceFromGoteHand(int fromIndex, Vector2Int toPos)
    {
        Piece piece = goteHandPieces[fromIndex];
        if (piece == null) return null;

        Piece targetPiece = pieceBoard[toPos.x, toPos.y];

        pieceBoard[toPos.x, toPos.y] = piece;

        return targetPiece;
    }

    // -------------------------
    // 持ち駒
    // -------------------------
    public void AddToHand(Vector2Int pos)
    {
        Piece piece = pieceBoard[pos.x, pos.y];
        if (piece == null) return;
        if (piece.data.team == Team.Sente)
        {
            Piece goteHandPiece = pieceFactory.GetPiece(Team.Gote, piece.data.type, false, true);
            goteHandPieces.Add(goteHandPiece);
        }
        else
        {
            Piece senteHandPiece = pieceFactory.GetPiece(Team.Sente, piece.data.type, false, true);
            senteHandPieces.Add(senteHandPiece);
        }
    }

    public void TestAddToHand()
    {
        for(int x = 0; x < 9; x++)
        {
            AddToHand(new Vector2Int(x,0));
            AddToHand(new Vector2Int(x,8));
        }
    }

    public void DebugPrintPieceBoard()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("=== Piece Board ===");

        for (int y = 8; y >= 0; y--)
        {
            for (int x = 0; x < 9; x++)
            {
                Piece piece = pieceBoard[x, y];

                // Piece自体がnull
                if (piece == null)
                {
                    sb.Append(" ・ ");
                    continue;
                }

                // dataがnull
                if (piece.data == null)
                {
                    sb.Append(" ?? ");
                    continue;
                }

                // team/typeが正常な場合
                string team = piece.data.team == Team.Sente ? "先" : "後";
                string type = piece.data.type.ToString().Substring(0, 1);

                sb.Append($"{team}{type}");

                if (piece.isPromoted)
                    sb.Append("*");
                else
                    sb.Append(" ");
            }

            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }

    // -------------------------
    // セル管理
    // -------------------------
    public void ClearCells()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                cellBoard[x, y]?.SetNormal();
            }
        }
        for (int y = 0; y < 10; y++)
        {
            for(int x = 0; x < 2; x++)
            {
                senteHandCells[x, y]?.SetNormal();
                goteHandCells[x, y]?.SetNormal();
            }
        }
    }

    public void ChangeBoardCells(List<Vector2Int> positions)
    {
        foreach (var pos in positions)
        {
            cellBoard[pos.x, pos.y].SetPlaceable();
        }
    }
    public void ChangeBoardCellSelected(Vector2Int pos)
    {
        cellBoard[pos.x, pos.y].SetSelected();
    }

    public void ChangeSenteHandCellSelected(Vector2Int pos)
    {
        senteHandCells[pos.x, pos.y].SetSelected();
    }
    public void ChangeGoteHandCellSelected(Vector2Int pos)
    {
        goteHandCells[pos.x, pos.y].SetSelected();
    }
    public List<Vector2Int> GetHandPieceMovablePositions()
    {
        List<Vector2Int> result = new List<Vector2Int>();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (pieceBoard[x, y] == null)
                {
                    result.Add(new Vector2Int(x, y));
                }
            }
        }

        return result;
    }

    public List<Vector2Int> GetBoardPieceMovablePositions(Vector2Int pos)
    {
        Piece piece = pieceBoard[pos.x, pos.y];
        if (piece == null) return new List<Vector2Int>();

        MovePattern movePattern = piece.GetMovePattern();

        Vector2Int[] direction = movePattern.direction;
        Vector2Int[] position = movePattern.position;

        List<Vector2Int> result = new List<Vector2Int>();

        // -------------------------
        // 方向（伸びる移動）
        // -------------------------
        foreach (var vec in direction)
        {
            for (int i = 1; i < 9; i++)
            {
                Vector2Int targetPos = pos + vec * i;

                if (!IsInsideBoard(targetPos))
                    break;
                // 駒があるなら止める
                Piece targetPiece = pieceBoard[targetPos.x, targetPos.y];
                if (targetPiece != null)
                {
                    if(targetPiece.data.team == piece.data.team)
                    {
                        break;
                    }
                    else
                    {
                        result.Add(targetPos);
                        break;
                    }
                }

                result.Add(targetPos);
            }
        }

        // -------------------------
        // 固定移動
        // -------------------------
        foreach (var vec in position)
        {
            Vector2Int target = pos + vec;

            if (IsInsideBoard(target))
            {
                if (pieceBoard[target.x, target.y] == null)
                {
                    result.Add(target);
                }
            }
        }

        return result;
    }

    // -------------------------
    // 判定
    // -------------------------
    public bool IsInsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 9 &&
               pos.y >= 0 && pos.y < 9;
    }

    public void DebugPrintCellBoard()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine("=== Cell Board ===");

        for (int y = 8; y >= 0; y--)
        {
            for (int x = 0; x < 9; x++)
            {
                Cell cell = cellBoard[x, y];

                if (cell == null)
                {
                    sb.Append(" ? ");
                }
                else
                {
                    switch (cell.state)
                    {
                        case CellState.Normal:
                            sb.Append(" . ");
                            break;

                        case CellState.Selected:
                            sb.Append(" S ");
                            break;

                        case CellState.Placeable:
                            sb.Append(" P ");
                            break;
                    }
                }
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }

    public Piece[,] GetPieceBoard()
    {
        return pieceBoard;
    }
    public Cell[,] GetCellBoard()
    {
        return cellBoard;
    }
    public List<Piece> GetSenteHandPieces()
    {
        return senteHandPieces;
    }
    public Cell[,] GetSenteHandCells()
    {
        return senteHandCells;
    }

    public List<Piece> GetGoteHandPieces()
    {
        return goteHandPieces;
    }
    public Cell[,] GetGoteHandCells()
    {
        return goteHandCells;
    }
}