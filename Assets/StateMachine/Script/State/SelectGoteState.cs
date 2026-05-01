using UnityEngine;

public class SelectGoteState : State
{
    public SelectGoteState(GameContext context) : base(context){}

    public override void Enter()
    {
        Debug.Log("SelectGoteStateに入りました");
        context.viewer.BuildAll();
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
            if (context.manager.IsPlaceable(boardPos))
            {
                context.manager.MoveFromGoteHand(boardPos);
                context.turn.ChangeTurn();
                context.manager.ClearCells();

                context.machine.ChangeState(new JudgeState(context));
                return;
            }

            // 盤面駒選択
            if (context.manager.SelectBoardPiece(boardPos))
            {
                if (context.manager.GetSelectedPieceTeam() == context.turn.GetCurrentTurn())
                {
                    context.manager.ChangeCellsByBoardPiece(boardPos);
                    context.machine.ChangeState(new SelectBoardState(context));
                }
                return;
            }
        }

        // --- 先手持ち駒 ---
        if (BoardConverter.WorldToSenteHand(pos, out Vector2Int senteHandPos))
        {
            if (context.manager.SelectSentePiece(senteHandPos))
            {
                if (context.manager.GetSelectedPieceTeam() == context.turn.GetCurrentTurn())
                {
                    context.manager.ChangeCellsBySenteHandPiece(senteHandPos);
                    context.viewer.BuildSenteHand();

                    context.machine.ChangeState(new SelectSenteState(context));
                }
                return;
            }
        }

        // --- 後手持ち駒（再選択） ---
        if (BoardConverter.WorldToGoteHand(pos, out Vector2Int goteHandPos))
        {
            if (context.manager.SelectGotePiece(goteHandPos))
            {
                if (context.manager.GetSelectedPieceTeam() == context.turn.GetCurrentTurn())
                {
                    context.manager.ChangeCellsByGoteHandPiece(goteHandPos);
                    context.viewer.BuildGoteHand();
                    context.viewer.BuildBoard();
                }
                return;
            }
        }
    }
}