using UnityEngine;

public class NetworkJudgeState : State
{
    public NetworkJudgeState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        if (context.judge.IsEnd(out Team winner))
        {
            context.machine.ChangeState(new EndState(context, winner));
        }
        else
        {
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
    {}

    private bool IsNetworkConnected()
    {
        return true;
    }
}