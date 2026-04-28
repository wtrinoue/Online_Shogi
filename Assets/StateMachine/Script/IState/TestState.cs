using UnityEngine;
using System.Collections.Generic;

public class TestState : IState
{
    public void Enter()
    {
        
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        Debug.Log(pos);
        Debug.Log("セルを更新したよ");
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            Piece piece = GameManager.Instance.GetBoardPiece(boardPos);
            if(piece != null){
                GameManager.Instance.ClearCells();
                GameManager.Instance.SetSelectedBoardPiecePosition(boardPos);
                GameManager.Instance.ChangeBoardCellSelected(boardPos);
                List<Vector2Int> positions = GameManager.Instance.GetBoardPieceMovablePositions(boardPos);
                GameManager.Instance.ChangeBoardCells(positions);
                GameViewer.Instance.ReloadCellBoard();
                GameViewer.Instance.BuildCellBoard();
            }
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            Piece piece = GameManager.Instance.GetSenteHandPiece(senteHandPos);
            if(piece != null)
            {
                GameManager.Instance.ClearCells();
                GameManager.Instance.SetSelectedSenteHandPiecePosition(senteHandPos);
                GameManager.Instance.ChangeSenteHandCellSelected(senteHandPos);
                List<Vector2Int> positions = GameManager.Instance.GetHandPieceMovablePositions();
                GameManager.Instance.ChangeBoardCells(positions);
                GameViewer.Instance.ReloadSenteHandCells();
                GameViewer.Instance.BuildSenteHandCells();
                GameViewer.Instance.ReloadCellBoard();
                GameViewer.Instance.BuildCellBoard();
            }
        }
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            Piece piece = GameManager.Instance.GetGoteHandPiece(goteHandPos);
            if(piece != null)
            {
                GameManager.Instance.ClearCells();
                GameManager.Instance.SetSelectedGoteHandPiecePosition(goteHandPos);
                GameManager.Instance.ChangeGoteHandCellSelected(goteHandPos);
                List<Vector2Int> positions = GameManager.Instance.GetHandPieceMovablePositions();
                GameManager.Instance.ChangeBoardCells(positions);
                GameViewer.Instance.ReloadGoteHandCells();
                GameViewer.Instance.BuildGoteHandCells();
                GameViewer.Instance.ReloadCellBoard();
                GameViewer.Instance.BuildCellBoard();
            }
        }
        return null;
    }
}
