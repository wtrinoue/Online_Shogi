using UnityEngine;

public class ModeState : State
{
    public ModeState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        // context.machine.ChangeState(new NetworkWaitState(context));
        context.machine.ChangeState(
            new TimerTextState(
                context,
                $"{context.turn.GetCurrentTurn()}のターン",
                1f,
                new IdleState(context)
            )
        );
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}
