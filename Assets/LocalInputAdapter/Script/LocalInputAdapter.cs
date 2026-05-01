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

        // スクリーン座標（Vector2）
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2 pos = new Vector2(worldPos.x, worldPos.y);
        OnClickEvent?.Invoke(pos);
    }
}