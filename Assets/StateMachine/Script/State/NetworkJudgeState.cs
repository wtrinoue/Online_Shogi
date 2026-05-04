using UnityEngine;

public class NetworkJudgeState : State
{
    public NetworkJudgeState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {}

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