using UnityEngine;

public class SelectBoardState : State
{
    public SelectBoardState(StateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        Debug.Log("SelectBoardStateに入りました");
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
            // 駒移動
            if (StateModule.Manager.IsPlaceable(boardPos))
            {
                StateModule.Manager.MoveFromBoard(boardPos);
                StateModule.Turn.ChangeTurn();
                StateModule.Manager.ClearCells();

                stateMachine.ChangeState(new JudgeState(stateMachine));
                return;
            }

            // 同じ盤面駒を再選択
            if (StateModule.Manager.SelectBoardPiece(boardPos))
            {
                if (StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsByBoardPiece(boardPos);
                    StateModule.Viewer.BuildBoard();
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

                    stateMachine.ChangeState(new SelectSenteState(stateMachine));
                }
                return;
            }
        }

        // --- 後手持ち駒 ---
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (StateModule.Manager.SelectGotePiece(goteHandPos))
            {
                if (StateModule.Manager.GetSelectedPieceTeam() == StateModule.Turn.GetCurrentTurn())
                {
                    StateModule.Manager.ChangeCellsByGoteHandPiece(goteHandPos);

                    stateMachine.ChangeState(new SelectGoteState(stateMachine));
                }
                return;
            }
        }
    }
}