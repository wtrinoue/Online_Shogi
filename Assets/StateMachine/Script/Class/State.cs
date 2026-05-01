using UnityEngine;
public abstract class State
{
    protected StateMachine stateMachine;

    protected State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void OnClick(Vector2 pos);
}