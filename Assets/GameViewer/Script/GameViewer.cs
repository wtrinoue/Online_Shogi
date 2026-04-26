using System.Collections.Generic;
using UnityEngine;

public class GameViewer : MonoBehaviour
{
    public PieceView pieceViewPrefab;
    public CellView cellViewPrefab;
    private GameViewer Instance;
    private Piece[,] pieceBoard = new Piece[9,9];
    private Cell[,] cellBoard = new Cell[9,9];
    private List<Piece> senteHandPieces = new List<Piece>();
    private List<Piece> goteHandPieces = new List<Piece>();
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gameManager = GameManager.Instance;
        Reload();
    }

    public void Reload()
    {
        pieceBoard = gameManager.GetPieceBoard();
        cellBoard = gameManager.GetCellBoard();
        senteHandPieces = gameManager.GetSenteHandPieces();
        goteHandPieces = gameManager.GetGoteHandPieces();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
