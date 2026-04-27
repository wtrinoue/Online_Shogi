using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class LocalInputAdapter : MonoBehaviour, IInputProvider, InputSystemActions.IShogiActions
{
    public event Action<Vector2> OnClickEvent;

    private InputSystemActions inputActions;

    private void Awake()
    {
        inputActions = new InputSystemActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Shogi.SetCallbacks(this);
    }

    private void OnDisable()
    {
        inputActions.Shogi.RemoveCallbacks(this);
        inputActions.Disable();
    }

    // Input System callback
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Debug.Log("クリックされました");

        // ★ここが修正ポイント
        Vector2 pos = Mouse.current.position.ReadValue();

        OnClickEvent?.Invoke(pos);
        Debug.Log(pos);
    }
}