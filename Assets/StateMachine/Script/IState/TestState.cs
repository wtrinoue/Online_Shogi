using UnityEngine;
using System.Collections.Generic;

public class TestState : IState
{
    public void Enter()
    {
        
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return null;
    }
}
