using UnityEngine;

public class NetworkJudgeState : State
{
    private State nextState;
    public NetworkJudgeState(GameContext context,State nextState) : base(context)
    {
        this.nextState = nextState;
    }

    public override void Enter()
    {
        Debug.Log("NetworkJudgeStateに入りました");
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
                    nextState
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