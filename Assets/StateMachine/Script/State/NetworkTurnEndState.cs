using UnityEngine;

public class NetworkTurnEndState : State
{
    public NetworkTurnEndState(GameContext context) : base(context){}

    public override void Enter()
    {
        Debug.Log("NetworkTurnEndState entered");

        Team movedTeam = context.turn.GetCurrentTurn() == Team.Sente
            ? Team.Gote
            : Team.Sente;

        context.manager.SignalMove(movedTeam);
        context.machine.ChangeState(new NetworkWaitState(context));
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }

    private bool IsNetworkConnected()
    {
        return true;
    }
}
