using UnityEngine;

public class TestState : IState
{
    public void Enter(){}
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        Debug.Log(pos);
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos)){
            GameManager.Instance.InitializeCell();
            GameManager.Instance.ChangeCellsByPiece(boardPos);
            GameViewer.Instance.BuildCellBoard();
            // Debug.Log(boardPos);
        }
        return null;
    }
}
