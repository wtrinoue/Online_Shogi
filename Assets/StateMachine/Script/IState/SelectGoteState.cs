using UnityEngine;
using System.Collections.Generic;
public class SelectGoteState : IState
{
    public void Enter()
    {
        Debug.Log("SelectGoteStateに入りました");
        StateModule.Viewer.BuildAll();
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if(StateModule.Manager.IsPlaceable(boardPos))
            {
                StateModule.Manager.MoveFromGoteHand(boardPos);
                StateModule.Turn.ChangeTurn();
                StateModule.Manager.ClearCells();
                return new TextState($"{StateModule.Turn.GetCurrentTurn()}のターン", new IdleState());
            }
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
            if (StateModule.Manager.SelectSentePiece(senteHandPos))
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
            if (StateModule.Manager.SelectGotePiece(goteHandPos))
            {
                if(StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsByGoteHandPiece(goteHandPos);
                    StateModule.Viewer.BuildGoteHand();
                    StateModule.Viewer.BuildBoard();
                    return null;
                }
            }
        }
        return null;
    }
}
