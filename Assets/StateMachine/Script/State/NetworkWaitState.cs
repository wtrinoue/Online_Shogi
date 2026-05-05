using UnityEngine;

public class NetworkWaitState : State
{
    public NetworkWaitState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        Debug.Log("NetworkWaitStateに入りました。相手の手を待っています…");
        context.machine.ChangeState(new NetworkJudgeState(context));
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}
