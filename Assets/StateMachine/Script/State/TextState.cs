using UnityEngine;

public class TextState : State
{
    private string message;
    private State nextState;

    public TextState(GameContext context, string message, State nextState)
        : base(context)
    {
        this.message = message;
        this.nextState = nextState;
    }

    public override void Enter()
    {
        Debug.Log("TextStateに入りました");
        context.text.Show(message);
        context.viewer.BuildAll();
    }

    public override void Exit()
    {
        context.text.Hide();
    }

    public override void OnClick(Vector2 pos)
    {
        context.machine.ChangeState(nextState);
    }
}