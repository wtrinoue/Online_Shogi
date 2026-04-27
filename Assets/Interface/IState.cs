using UnityEngine;

public interface IState
{
    void Enter();
    void Exit();
    IState OnClick(Vector2 pos);
}
