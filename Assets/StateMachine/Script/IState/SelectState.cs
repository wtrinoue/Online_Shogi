using UnityEngine;

public class SelectState : IState
{
    public void Enter()
    {
        Debug.Log("SelectStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            return new BoardMoveState();
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            return new SenteMoveState();
        }
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            return new GoteMoveState();
        }
        return null;
    }
}
