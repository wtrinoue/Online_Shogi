using UnityEngine;
using System.Collections;

public class NetworkWaitState : State
{
    public NetworkWaitState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        Debug.Log("NetworkWaitStateに入りました。相手の手を待っています…");
        context.manager.ChangeIsMovedTo(false);
        context.machine.RunCoroutine(WaitForMoveCoroutine());
    }

    private IEnumerator WaitForMoveCoroutine()
    {
        while (context.manager.GetIsMoved())
        {
            Debug.Log("一生はまっています");
            context.manager.ClearCells();
            context.viewer.BuildAll();
            yield return  new WaitForSeconds(0.5f);
        }

        context.machine.ChangeState(new NetworkJudgeState(context));
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}
