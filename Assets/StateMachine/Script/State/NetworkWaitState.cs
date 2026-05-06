using UnityEngine;
using System.Collections;

public class NetworkWaitState : State
{
    public NetworkWaitState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        Debug.Log("NetworkWaitState entered. Waiting for opponent move.");
        context.text.Show("相手の手を待っています...");
        context.machine.RunCoroutine(WaitForMoveCoroutine());
    }

    private IEnumerator WaitForMoveCoroutine()
    {
        int lastMoveSignal = context.manager.GetMoveSignal();
        Team waitingTeam = context.turn.GetCurrentTurn();

        while (context.manager.GetMoveSignal() == lastMoveSignal ||
               context.manager.GetLastMovedTeam() != waitingTeam)
        {
            Debug.Log($"Waiting for move... signal={context.manager.GetMoveSignal()}, waitingTeam={waitingTeam}, lastMovedTeam={context.manager.GetLastMovedTeam()}");
            context.viewer.BuildAll();
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Opponent move detected");
        context.machine.ChangeState(new NetworkJudgeState(context));
    }

    public override void Exit()
    {
        context.text.Hide();
    }

    public override void OnClick(Vector2 pos)
    {
    }
}
