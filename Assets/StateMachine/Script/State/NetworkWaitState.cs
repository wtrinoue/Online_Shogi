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
        context.text.Show("相手が手を打っています");
        context.manager.ChangeIsMovedTo(false);
        context.machine.RunCoroutine(WaitForMoveCoroutine());
    }

    private IEnumerator WaitForMoveCoroutine()
    {
        while (!context.manager.GetIsMoved())
        {
            Debug.Log("相手の手を待機中...");
            context.viewer.BuildAll();
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("相手の手を検知しました");
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
