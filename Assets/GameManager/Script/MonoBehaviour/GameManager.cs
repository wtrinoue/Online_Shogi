using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<PiecePlacement> piecePlacementList;

    // ★ ここを配列に変更
    public Piece[,] pieceBoard = new Piece[9, 9];
    public Cell[,] cellBoard = new Cell[9, 9];

    public List<Piece> senteHandPieces = new List<Piece>();
    public List<Piece> goteHandPieces = new List<Piece>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // -------------------------
    // 初期化
    // -------------------------
    public void InitializePiece()
    {
        foreach (var piecePlacement in piecePlacementList)
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
    }

    // -------------------------
    // 成り判定
    // -------------------------
    public void PromotePiece(Vector2Int pos)
    {
        Piece piece = pieceBoard[pos.x, pos.y];
        if (piece == null) return;

        if (piece.isPromoted) return;

        Team team = piece.data.team;

        if (team == Team.Sente)
        {
            if (pos.y >= 6)
                piece.Promote();
        }
        else
        {
            if (pos.y <= 2)
                piece.Promote();
        }
    }

    // -------------------------
    // 移動
    // -------------------------
    public void MovePiece(Vector2Int fromPos, Vector2Int toPos)
    {
        Piece piece = pieceBoard[fromPos.x, fromPos.y];
        if (piece == null) return;

        Piece targetPiece = pieceBoard[toPos.x, toPos.y];

        if (targetPiece != null)
        {
            // 将来：持ち駒処理
        }

        pieceBoard[toPos.x, toPos.y] = piece;
        pieceBoard[fromPos.x, fromPos.y] = null;

        PromotePiece(toPos);

        ClearCell();
        ChangeCellsByPiece(toPos);
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
            goteHandPieces.Add(new Piece(piece.data, false, true));
        }
        else
        {
            senteHandPieces.Add(new Piece(piece.data, false, true));
        }
    }

    // -------------------------
    // セル管理
    // -------------------------
    public void ClearCell()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                cellBoard[x, y]?.SetNormal();
            }
        }
    }

    public void ChangeCellsByPiece(Vector2Int pos)
    {
        Piece piece = pieceBoard[pos.x, pos.y];
        if (piece == null) return;

        MovePattern movePattern = piece.GetMovePattern();

        Vector2Int[] direction = movePattern.direction;
        Vector2Int[] position = movePattern.position;

        List<Vector2Int> checkPoints = new List<Vector2Int>();

        // -------------------------
        // 方向（伸びる移動）
        // -------------------------
        foreach (var vec in direction)
        {
            for (int i = 1; i < 9; i++)
            {
                Vector2Int target = pos + vec * i;

                if (!IsInsideBoard(target))
                    break;

                // 駒があるなら止める
                if (pieceBoard[target.x, target.y] != null)
                {
                    checkPoints.Add(target);
                    break;
                }

                checkPoints.Add(target);
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
                    checkPoints.Add(target);
                }
            }
        }

        // -------------------------
        // セル反映
        // -------------------------
        foreach (var vec in checkPoints)
        {
            cellBoard[vec.x, vec.y].SetPlaceable();
        }

        cellBoard[pos.x, pos.y].SetSelected();
    }

    // -------------------------
    // 判定
    // -------------------------
    public bool IsInsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 9 &&
               pos.y >= 0 && pos.y < 9;
    }
}