using UnityEngine;

public class EmptyState : State
{
    public EmptyState(GameContext context) : base(context){}

    public override void Enter(){}
    public override void Exit(){}
    public override void OnClick(Vector2 pos){}
}