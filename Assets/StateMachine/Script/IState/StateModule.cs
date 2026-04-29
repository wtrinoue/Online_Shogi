using UnityEngine;
using System.Collections.Generic;
public static class StateModule
{
    public static bool SelectBoardPiece(Vector2Int pos)
    {
        Piece piece = GameManager.Instance.GetBoardPiece(pos);
        if(piece != null){
            GameManager.Instance.ClearCells();
            GameManager.Instance.SetSelectedBoardPiecePosition(pos);
            GameManager.Instance.ChangeBoardCellSelected(pos);
            List<Vector2Int> positions = GameManager.Instance.GetBoardPieceMovablePositions(pos);
            GameManager.Instance.ChangeBoardCells(positions);
            GameViewer.Instance.ReloadCellBoard();
            GameViewer.Instance.BuildCellBoard();
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool SelectSentePiece(Vector2Int pos)
    {
        Piece piece = GameManager.Instance.GetSenteHandPiece(pos);
        if(piece != null)
        {
            GameManager.Instance.ClearCells();
            GameManager.Instance.SetSelectedSenteHandPiecePosition(pos);
            GameManager.Instance.ChangeSenteHandCellSelected(pos);
            List<Vector2Int> positions = GameManager.Instance.GetHandPieceMovablePositions();
            GameManager.Instance.ChangeBoardCells(positions);
            GameViewer.Instance.ReloadSenteHandCells();
            GameViewer.Instance.BuildSenteHandCells();
            GameViewer.Instance.ReloadCellBoard();
            GameViewer.Instance.BuildCellBoard();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool SelectGotePiece(Vector2Int pos)
    {
        Piece piece = GameManager.Instance.GetGoteHandPiece(pos);
        if(piece != null)
        {
            GameManager.Instance.ClearCells();
            GameManager.Instance.SetSelectedGoteHandPiecePosition(pos);
            GameManager.Instance.ChangeGoteHandCellSelected(pos);
            List<Vector2Int> positions = GameManager.Instance.GetHandPieceMovablePositions();
            GameManager.Instance.ChangeBoardCells(positions);
            GameViewer.Instance.ReloadGoteHandCells();
            GameViewer.Instance.BuildGoteHandCells();
            GameViewer.Instance.ReloadCellBoard();
            GameViewer.Instance.BuildCellBoard();
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsPlaceable(Vector2Int pos)
    {
        return GameManager.Instance.IsPlaceableOnBoard(pos);
    }
    public static void MoveFromBoard(Vector2Int pos)
    {
        Vector2Int selectedPos = GameManager.Instance.GetSelectedBoardPiecePosition();
        Piece targetPiece = GameManager.Instance.MovePieceFromBoard(selectedPos, pos);
        if(targetPiece != null)
        {
            GameManager.Instance.AddToHand(targetPiece);
        }
        if(GameManager.Instance.IsPromotable(pos))
        {
            GameManager.Instance.PromotePiece(pos);
        }
    }
    public static void MoveFromSenteHand(Vector2Int pos)
    {
        Vector2Int selectedPos = GameManager.Instance.GetSelectedSenteHandPiecePosition();
        Piece targetPiece = GameManager.Instance.MovePieceFromSenteHand(selectedPos, pos);
        if(targetPiece != null)
        {
            GameManager.Instance.AddToHand(targetPiece);
        }
        if (GameManager.Instance.IsPromotable(pos))
        {
            GameManager.Instance.PromotePiece(pos);
        }
    }

    public static void MoveFromGoteHand(Vector2Int pos)
    {
        Vector2Int selectedPos = GameManager.Instance.GetSelectedGoteHandPiecePosition();
        Piece targetPiece = GameManager.Instance.MovePieceFromGoteHand(selectedPos, pos);
        if(targetPiece != null)
        {
            GameManager.Instance.AddToHand(targetPiece);
        }
        if (GameManager.Instance.IsPromotable(pos))
        {
            GameManager.Instance.PromotePiece(pos);
        }
    }
}