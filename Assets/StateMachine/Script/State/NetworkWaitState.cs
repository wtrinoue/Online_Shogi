using UnityEngine;

public class NetworkWaitState : State
{
    public NetworkWaitState(GameContext context) : base(context)
    {
        context.machine.ChangeState(new NetworkJudgeState(context));
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}
