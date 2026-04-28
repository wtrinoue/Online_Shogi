using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("IdleStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        Debug.Log("クリックされたよ");
        return new TextState("SelectStateに移行",new SelectState());
    }
}
