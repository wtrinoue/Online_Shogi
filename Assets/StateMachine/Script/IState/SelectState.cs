using UnityEngine;

public class SelectState : IState
{
    public void Enter()
    {
        Debug.Log("SelectStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return new BoardMoveState();
    }
}
