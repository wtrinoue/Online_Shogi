using UnityEngine;

public class SenteEntryState : State
{
    public SenteEntryState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        context.turn.SetTurn(Team.Sente);
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
