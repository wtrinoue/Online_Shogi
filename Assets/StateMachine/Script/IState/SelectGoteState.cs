using UnityEngine;

public class SelectGoteState : State
{
    public SelectGoteState(StateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        Debug.Log("SelectGoteStateに入りました");
        StateModule.Viewer.BuildAll();
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
        // --- 盤面クリック ---
        if (BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            // 駒を打つ
            if (StateModule.Manager.IsPlaceable(boardPos))
            {
                StateModule.Manager.MoveFromGoteHand(boardPos);
                StateModule.Turn.ChangeTurn();
                StateModule.Manager.ClearCells();

                stateMachine.ChangeState(
                    new TextState(
                        stateMachine,
                        $"{StateModule.Turn.GetCurrentTurn()}のターン",
                        new IdleState(stateMachine)
                    )
                );
                return;
            }

            // 盤面駒選択
            if (StateModule.Manager.SelectBoardPiece(boardPos))
            {
                if (StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsByBoardPiece(boardPos);
                    stateMachine.ChangeState(new SelectBoardState(stateMachine));
                }
                return;
            }
        }

        // --- 先手持ち駒 ---
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (StateModule.Manager.SelectSentePiece(senteHandPos))
            {
                if (StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsBySenteHandPiece(senteHandPos);
                    StateModule.Viewer.BuildSenteHand();

                    stateMachine.ChangeState(new SelectSenteState(stateMachine));
                }
                return;
            }
        }

        // --- 後手持ち駒（再選択） ---
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (StateModule.Manager.SelectGotePiece(goteHandPos))
            {
                if (StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsByGoteHandPiece(goteHandPos);
                    StateModule.Viewer.BuildGoteHand();
                    StateModule.Viewer.BuildBoard();
                }
                return;
            }
        }
    }
}