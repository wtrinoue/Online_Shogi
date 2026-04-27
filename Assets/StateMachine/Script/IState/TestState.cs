using UnityEngine;

public class TestState : IState
{
    public void Enter(){}
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        Debug.Log("TestStateが実行されたよ！");
        return null;
    }
}
