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
        GameManager.Instance.ClearCells();
        GameViewer.Instance.ReloadCellBoard();
        GameViewer.Instance.BuildCellBoard();
        Debug.Log("セルを更新したよ");
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            GameManager.Instance.ClearCells();
            GameManager.Instance.ChangeBoardCellSelected(boardPos);
            List<Vector2Int> positions = GameManager.Instance.GetBoardPieceMovablePositions(boardPos);
            GameManager.Instance.ChangeBoardCells(positions);
            GameViewer.Instance.ReloadCellBoard();
            GameViewer.Instance.BuildCellBoard();
        }
        return null;
    }
}
