using UnityEngine;

public class TextState : IState
{
    public void Enter()
    {
        Debug.Log("TextStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return null;
    }
}
