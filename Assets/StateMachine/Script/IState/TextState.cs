using UnityEngine;

public class TextState : IState
{
    private string message;
    private IState nextState;

    public TextState(string message, IState nextState)
    {
        this.message = message;
        this.nextState = nextState;
    }

    public void Enter()
    {
        Debug.Log("TextStateに入りました");
        TextManager.Instance.Show(message);
        StateModule.Viewer.BuildAll();
    }

    public void Exit()
    {
        TextManager.Instance.Hide();
    }

    public IState OnClick(Vector2 pos)
    {
        // クリックで次の状態へ
        return nextState;
    }
}