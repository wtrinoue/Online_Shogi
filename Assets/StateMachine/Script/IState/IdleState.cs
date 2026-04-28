using UnityEngine;

public class IdleState : IState
{
    public void Enter(){}
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return new SelectState();
    }
}
