using UnityEngine;

public interface IState
{
    void Enter();
    void Exit();
    void OnClick(Vector2 pos);
}
