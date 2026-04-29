using UnityEngine;
using System.Collections.Generic;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("IdleStateに入りました");
        StateModule.Manager.ClearCells();
        StateModule.Viewer.BuildAll();
    }
    public void Exit()
    {
        StateModule.Viewer.BuildAll();
    }
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if(StateModule.Manager.SelectBoardPiece(boardPos))
            {
                if(StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsByBoardPiece(boardPos);
                    StateModule.Viewer.BuildBoard();
                    return new SelectBoardState();
                }
            }
        }
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if(StateModule.Manager.SelectSentePiece(senteHandPos))
            {
                if(StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsBySenteHandPiece(senteHandPos);
                    StateModule.Viewer.BuildSenteHand();
                    return new SelectSenteState();
                }
            }
        }
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if(StateModule.Manager.SelectGotePiece(goteHandPos))
            {
                if(StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsByGoteHandPiece(goteHandPos);
                    StateModule.Viewer.BuildGoteHand();
                    return new SelectGoteState();
                }
            }
        }
        return null;
    }
}
