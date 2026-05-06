using UnityEngine;
public class NetworkTurnEndState : State
{
    public NetworkTurnEndState(GameContext context) : base(context){}

    public override void Enter()
    {
        Debug.Log("NetworkTurnEndStateに入りました");
        context.manager.SignalMove();
        // クライアントに通知
        var networkGM = context.manager as NetworkGameManager;
        if (networkGM != null)
        {
            networkGM.RPC_NotifyMoveCompleted();
        }
        context.machine.ChangeState(new NetworkWaitState(context));
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