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
        // ホストの場合はカウンタを待つ、クライアントの場合は RPC を待つ
        if (Object.HasStateAuthority)
        {
            context.machine.RunCoroutine(WaitForMoveCoroutine());
        }
        // クライアントは RPC で通知される
    }

    private IEnumerator WaitForMoveCoroutine()
    {
        int lastMoveSignal = context.manager.GetMoveSignal();
        while (context.manager.GetMoveSignal() == lastMoveSignal)
        {
            Debug.Log($"相手の手を待機中... signal={context.manager.GetMoveSignal()}");
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
