using UnityEngine;

public class SelectState : IState
{
    public void Enter(){}
    public void Exit(){}
    public IState OnClick(Vector2 pos){return null;}
}
