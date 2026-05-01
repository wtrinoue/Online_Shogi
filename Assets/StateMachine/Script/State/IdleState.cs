using UnityEngine;

public class IdleState : State
{
    public IdleState(GameContext context) : base(context){}

    public override void Enter()
    {
        Debug.Log("IdleStateに入りました");

        context.manager.ClearCells();
        context.viewer.BuildAll();
    }

    public override void Exit()
    {
        context.viewer.BuildAll();
    }

    public override void OnClick(Vector2 pos)
    {
        // --- 盤面クリック ---
        if (BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if (context.manager.SelectBoardPiece(boardPos) &&
                context.turn.GetCurrentTurn() == context.manager.GetSelectedPieceTeam())
            {
                context.manager.ChangeCellsByBoardPiece(boardPos);
                context.viewer.BuildBoard();

                context.machine.ChangeState(new SelectBoardState(context));
                return;
            }
        }

        // --- 先手持ち駒 ---
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (context.manager.SelectSentePiece(senteHandPos) &&
                context.turn.GetCurrentTurn() == context.manager.GetSelectedPieceTeam())
            {
                context.manager.ChangeCellsBySenteHandPiece(senteHandPos);
                context.viewer.BuildSenteHand();

                context.machine.ChangeState(new SelectSenteState(context));
                return;
            }
        }

        // --- 後手持ち駒 ---
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (context.manager.SelectGotePiece(goteHandPos) &&
                context.turn.GetCurrentTurn() == context.manager.GetSelectedPieceTeam())
            {
                context.manager.ChangeCellsByGoteHandPiece(goteHandPos);
                context.viewer.BuildGoteHand();

                context.machine.ChangeState(new SelectGoteState(context));
                return;
            }
        }
    }
}