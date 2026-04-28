using UnityEngine;

public class BoardMoveState : IState
{
    public void Enter()
    {
        Debug.Log("GoteMoveStateに入りました、");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){return null;}
}
