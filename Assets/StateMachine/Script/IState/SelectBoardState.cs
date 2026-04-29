using UnityEngine;
using System.Collections.Generic;
public class SelectBoardState : IState
{
    public void Enter()
    {
        Debug.Log("SelectBoardStateに入りました");
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
                StateModule.Manager.MoveFromBoard(boardPos);
                return new IdleState();
            }
            if(StateModule.Manager.SelectBoardPiece(boardPos))
            {
                StateModule.Viewer.BuildBoard();
                return null;
            }
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (StateModule.Manager.SelectSentePiece(senteHandPos))
            {
                return new SelectSenteState();
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
