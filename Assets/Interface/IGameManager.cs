using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void Init();
    void InitializePiece();
    void InitializeCell();
    void PromotePiece(Vector2Int pos);
    bool IsPromotable(Vector2Int pos);
    Piece MovePieceFromBoard(Vector2Int fromPos, Vector2Int toPos);
    Piece MovePieceFromSenteHand(Vector2Int fromPos, Vector2Int toPos);
    Piece MovePieceFromGoteHand(Vector2Int fromPos, Vector2Int toPos);
    void AddToHand(Piece piece);
    void TestAddToHand();
    void ClearCells();
    void ChangeBoardCells(List<Vector2Int> positions);
    void ChangeBoardCellSelected(Vector2Int pos);
    void ChangeSenteHandCellSelected(Vector2Int pos);
    void ChangeGoteHandCellSelected(Vector2Int pos);
    List<Vector2Int> GetHandPieceMovablePositions();
    List<Vector2Int> GetBoardPieceMovablePositions(Vector2Int pos);
    bool IsInsideBoard(Vector2Int pos);
    bool IsPlaceableOnBoard(Vector2Int pos);
    void SetSelectedBoardPiecePosition(Vector2Int pos);
    void SetSelectedSenteHandPiecePosition(Vector2Int pos);
    void SetSelectedGoteHandPiecePosition(Vector2Int pos);
    Vector2Int GetSelectedBoardPiecePosition();
    Vector2Int GetSelectedSenteHandPiecePosition();
    Vector2Int GetSelectedGoteHandPiecePosition();
    Piece GetSelectedPiece();
    Piece GetBoardPiece(Vector2Int pos);
    Piece GetSenteHandPiece(Vector2Int pos);
    Piece GetGoteHandPiece(Vector2Int pos);
    Cell GetBoardCell(Vector2Int pos);
    Piece[,] GetPieceBoard();
    Cell[,] GetCellBoard();
    List<Piece> GetSenteHandPieces();
    Cell[,] GetSenteHandCells();
    List<Piece> GetGoteHandPieces();
    Cell[,] GetGoteHandCells();
    void DebugPrintPieceBoard();
    void DebugPrintCellBoard();
    bool IsGameOver(out Team winner);
    void SignalMove();
    int GetMoveSignal();
    void ChangeIsMovedTo(bool isMoved);
    bool GetIsMoved();
}