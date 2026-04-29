using UnityEngine;
using System.Collections.Generic;
public class SelectGoteState : IState
{
    public void Enter()
    {
        GameManager.Instance.ClearCells();
        GameViewer.Instance.ReloadAllData();
        GameViewer.Instance.BuildAll();
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if(StateModule.IsPlaceable(boardPos))
            {
                StateModule.MoveFromGoteHand(boardPos);
                return new IdleState();
            }else if(StateModule.SelectBoardPiece(boardPos))
            {
                return new SelectBoardState();
            }
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (StateModule.SelectSentePiece(senteHandPos))
            {
                return new SelectSenteState();
            }
        }
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (StateModule.SelectGotePiece(goteHandPos))
            {
                return null;
            }
        }
        return null;
    }
}
