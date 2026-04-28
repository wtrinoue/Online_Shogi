using UnityEngine;

public class BoardMoveState : IState
{
    public void Enter()
    {
        Debug.Log("BoardMoveStateに入りました、");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return new IdleState();
    }
}
