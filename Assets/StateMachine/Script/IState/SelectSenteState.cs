using UnityEngine;
using System.Collections.Generic;
public class SelectSenteState : IState
{
    public void Enter()
    {
        Debug.Log("SelectSenteStateに入りました");
        StateModule.BuildAll();
    }
    public void Exit()
    {
        StateModule.BuildAll();
    }
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if(StateModule.IsPlaceable(boardPos))
            {
                StateModule.MoveFromSenteHand(boardPos);
                return new IdleState();
            }
            if(StateModule.SelectBoardPiece(boardPos))
            {
                return new SelectBoardState();
            }
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (StateModule.SelectSentePiece(senteHandPos))
            {
                return null;
            }
        }
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (StateModule.SelectGotePiece(goteHandPos))
            {
                return new SelectGoteState();
            }
        }
        return null;
    }
}
