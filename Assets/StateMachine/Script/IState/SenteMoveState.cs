using UnityEngine;

public class SenteMoveState : IState
{
    public void Enter()
    {
        Debug.Log("SenteMoveStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return null;
    }
}
