using UnityEngine;
public abstract class State
{
    protected GameContext context;

    public State(GameContext context)
    {
        this.context = context;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void OnClick(Vector2 pos);
}