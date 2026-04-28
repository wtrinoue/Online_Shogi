using UnityEngine;

public class GoteMoveState : IState
{
    public void Enter()
    {
        Debug.Log("GoteMoveStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return new IdleState();
    }
}
