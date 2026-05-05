using UnityEngine;

public class GoteEntryState : State
{
    public GoteEntryState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        context.machine.ChangeState(new NetworkWaitState(context));
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}
