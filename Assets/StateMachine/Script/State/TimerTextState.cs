using UnityEngine;
using System.Collections;

public class TimerTextState : State
{
    private string message;
    private float timer;
    private State nextState;

    public TimerTextState(GameContext context, string message, float timer, State nextState)
        : base(context)
    {
        this.message = message;
        this.timer = timer;
        this.nextState = nextState;
    }

    public override void Enter()
    {
        Debug.Log("TimerTextStateに入りました");
        context.text.Show(message);
        context.viewer.BuildAll();
        context.machine.RunCoroutine(TimerCoroutine());
    }

    public override void Exit()
    {
        context.text.Hide();
    }

    public override void OnClick(Vector2 pos)
    {
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(timer);

        context.machine.ChangeState(nextState);
    }
}