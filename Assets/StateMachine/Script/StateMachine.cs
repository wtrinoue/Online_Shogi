using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;
    public void ChangeState(IState state)
    {
        this.currentState = state;
    }
    public void OnClick(Vector2 pos)
    {
        var next = currentState.OnClick(pos);

        if (next != null)
            ChangeState(next);
    }
}
