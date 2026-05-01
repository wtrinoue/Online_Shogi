using UnityEngine;
using System.Collections.Generic;
public class SelectState : IState
{
    public void Enter()
    {
        Debug.Log("SelectStateに入りました");
        GameManager.Instance.ClearCells();
        GameViewer.Instance.ReloadAllData();
        GameViewer.Instance.BuildAll();
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if (SelectBoardPiece(boardPos))
            {
                return new BoardMoveState();
            }
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (SelectSentePiece(senteHandPos))
            {
                return new SenteMoveState();
            }
        }
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (SelectGotePiece(goteHandPos))
            {
                return new GoteMoveState();
            }
        }
        return new SelectState();
    }
    public bool SelectBoardPiece(Vector2Int pos)
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
    public bool SelectSentePiece(Vector2Int pos)
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

    public bool SelectGotePiece(Vector2Int pos)
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
}
