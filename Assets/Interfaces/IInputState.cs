using UnityEngine;

public interface IInputState
{
    void Enter();
    void Exit();
    void OnClick(Vector2Int pos);
}