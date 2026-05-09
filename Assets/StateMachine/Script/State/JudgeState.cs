using UnityEngine;

public class JudgeState : State
{
    public JudgeState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        Debug.Log("JudgeStateに入りました");
        context.manager.ClearCells();
        context.viewer.BuildAll();
        if (context.judge.IsEnd(out Team winner))
        {
            context.machine.ChangeState(new EndState(context, winner));
        }
        else
        {
            context.turn.ChangeTurn();
            context.machine.ChangeState(
                    new TimerTextState(
                        context,
                        $"{context.turn.GetCurrentTurn()}のターン",
                        1f,
                        new IdleState(context)
                    )
            );
        }
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}