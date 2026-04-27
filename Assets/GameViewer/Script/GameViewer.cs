using System.Collections.Generic;
using UnityEngine;

public class GameViewer : MonoBehaviour
{
    public static GameViewer Instance;
    public PieceView pieceViewPrefab;
    public CellView cellViewPrefab;
    public BoardConfig boardConfig;
    private Vector3 boardStartPos = new Vector3(0f, 0f, 0f);
    private Vector3 senteHnadStartPos = new Vector3(0f, 0f, 0f);
    private Vector3 goteHandStartPos = new Vector3(0f, 0f, 0f);
    private Piece[,] pieceBoard = new Piece[9,9];
    private Cell[,] cellBoard = new Cell[9,9];
    private List<Piece> senteHandPieces = new List<Piece>();
    private List<Piece> goteHandPieces = new List<Piece>();
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameManager = GameManager.Instance;
        boardStartPos = boardConfig.boardOffset;
        senteHnadStartPos = boardConfig.senteHandOffset;
        goteHandStartPos = boardConfig.goteHandOffset;
        ReloadAllData();
        BuildAll();
    }

    public void ReloadAllData()
    {
        // 参照渡しなので二度は必要ない可能性がある。
        ReloadPieceBoard();
        ReloadCellBoard();
        ReloadSenteHandPieces();
        ReloadGoteHandPieces();
    }

    public void ReloadPieceBoard()
    {
        pieceBoard = gameManager.GetPieceBoard();
    }
    public void ReloadCellBoard()
    {
        cellBoard = gameManager.GetCellBoard();
    }
    public void ReloadSenteHandPieces()
    {
        senteHandPieces = gameManager.GetSenteHandPieces();
    }
    public void ReloadGoteHandPieces()
    {
        goteHandPieces = gameManager.GetGoteHandPieces();
    }
    public void BuildAll()
    {
        DeleteAll();
        BuildCellBoard();
        BuildPieceBoard();
        BuildSenteHandPieces();
        BuildGoteHandPieces();
    }

    public void DeleteAll()
    {
        // -------------------------
        // 既存の表示をクリア
        // -------------------------
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void BuildPieceBoard()
    {
        // -------------------------
        // PieceView生成（駒）
        // -------------------------
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Piece piece = pieceBoard[x, y];
                if (piece == null) continue;

                PieceView pieceView = Instantiate(pieceViewPrefab, transform);
                pieceView.transform.position = new Vector3(x + boardStartPos.x, y + boardStartPos.y, 0f); // 少し上に表示

                pieceView.SetPiece(piece);
            }
        }
    }

    public void BuildCellBoard()
    {
        // -------------------------
        // CellView生成（盤面）
        // -------------------------
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Cell cell = cellBoard[x, y];
                if (cell == null) continue;
                CellView cellView = Instantiate(cellViewPrefab, transform);
                cellView.transform.position = new Vector3(x + boardStartPos.x, y + boardStartPos.y, 0.1f);

                cellView.SetCell(cell);
                cellView.UpdateView();
            }
        }
    }

    public void BuildSenteHandPieces()
    {
        int count = senteHandPieces.Count;
        for(int i = 0; i < count; i++)
        {
            Piece piece = senteHandPieces[i];
            int x = i / 10;
            int y = i % 10;
            Debug.Log("持ち駒を追加しました");
            PieceView pieceView = Instantiate(pieceViewPrefab, transform);
            pieceView.transform.position = new Vector3(x + senteHnadStartPos.x, y + senteHnadStartPos.y, 0f); // 少し上に表示
            pieceView.SetPiece(piece);
        }
    }
    public void BuildGoteHandPieces()
    {
        int count = goteHandPieces.Count;
        for(int i = 0; i < count; i++)
        {
            Piece piece = goteHandPieces[i];
            int x = i / 10;
            int y = i % 10;
            Debug.Log("持ち駒を追加しました");
            PieceView pieceView = Instantiate(pieceViewPrefab, transform);
            pieceView.transform.position = new Vector3(x + goteHandStartPos.x, y + goteHandStartPos.y, 0f); // 少し上に表示
            pieceView.SetPiece(piece);
        }
    }
}
