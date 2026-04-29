using UnityEngine;
using System.Collections.Generic;
public class SelectSenteState : IState
{
    public void Enter()
    {
        Debug.Log("SelectSenteStateに入りました");
        StateModule.Viewer.BuildAll();
    }
    public void Exit()
    {
        StateModule.Viewer.BuildAll();
    }
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if(StateModule.Manager.IsPlaceable(boardPos))
            {
                StateModule.Manager.MoveFromSenteHand(boardPos);
                return new IdleState();
            }
            if(StateModule.Manager.SelectBoardPiece(boardPos))
            {
                return new SelectBoardState();
            }
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (StateModule.Manager.SelectSentePiece(senteHandPos))
            {
                StateModule.Viewer.BuildSenteHand();
                return null;
            }
        }
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (StateModule.Manager.SelectGotePiece(goteHandPos))
            {
                return new SelectGoteState();
            }
        }
        return null;
    }
}
