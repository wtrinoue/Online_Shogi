using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("IdleStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return new SelectState();
    }
}
