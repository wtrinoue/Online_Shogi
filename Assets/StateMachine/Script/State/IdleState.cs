using UnityEngine;

public class IdleState : State
{
    public IdleState(StateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        Debug.Log("IdleStateに入りました");

        StateModule.Manager.ClearCells();
        StateModule.Viewer.BuildAll();
        if(StateModule.Judge.IsEnd(out Team winner))
        {
            stateMachine.ChangeState(new EndState(stateMachine, winner));
        }
    }

    public override void Exit()
    {
        StateModule.Viewer.BuildAll();
    }

    public override void OnClick(Vector2 pos)
    {
        // --- 盤面クリック ---
        if (BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if (StateModule.Manager.SelectBoardPiece(boardPos) &&
                StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
            {
                StateModule.Manager.ChangeCellsByBoardPiece(boardPos);
                StateModule.Viewer.BuildBoard();

                stateMachine.ChangeState(new SelectBoardState(stateMachine));
                return;
            }
        }

        // --- 先手持ち駒 ---
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (StateModule.Manager.SelectSentePiece(senteHandPos) &&
                StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
            {
                StateModule.Manager.ChangeCellsBySenteHandPiece(senteHandPos);
                StateModule.Viewer.BuildSenteHand();

                stateMachine.ChangeState(new SelectSenteState(stateMachine));
                return;
            }
        }

        // --- 後手持ち駒 ---
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (StateModule.Manager.SelectGotePiece(goteHandPos) &&
                StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
            {
                StateModule.Manager.ChangeCellsByGoteHandPiece(goteHandPos);
                StateModule.Viewer.BuildGoteHand();

                stateMachine.ChangeState(new SelectGoteState(stateMachine));
                return;
            }
        }
    }
}