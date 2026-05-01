using UnityEngine;

public class TextState : State
{
    private string message;
    private State nextState;

    public TextState(StateMachine stateMachine, string message, State nextState)
        : base(stateMachine)
    {
        this.message = message;
        this.nextState = nextState;
    }

    public override void Enter()
    {
        Debug.Log("TextStateに入りました");
        TextManager.Instance.Show(message);
        StateModule.Viewer.BuildAll();
    }

    public override void Exit()
    {
        TextManager.Instance.Hide();
    }

    public override void OnClick(Vector2 pos)
    {
        stateMachine.ChangeState(nextState);
    }
}