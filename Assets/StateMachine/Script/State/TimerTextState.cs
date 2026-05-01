using UnityEngine;
using System.Collections;

public class TimerTextState : State
{
    private string message;
    private float timer;
    private State nextState;

    public TimerTextState(StateMachine stateMachine, string message,float timer, State nextState)
        : base(stateMachine)
    {
        this.message = message;
        this.timer = timer;
        this.nextState = nextState;
    }

    public override void Enter()
    {
        Debug.Log("TimerTextStateに入りました");
        TextManager.Instance.Show(message);
        StateModule.Viewer.BuildAll();
        stateMachine.RunCoroutine(TimerCoroutine());
    }

    public override void Exit()
    {
        TextManager.Instance.Hide();
    }

    public override void OnClick(Vector2 pos)
    {
    }
    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(timer);

        stateMachine.ChangeState(nextState);
    }
}