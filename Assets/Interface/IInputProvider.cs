using UnityEngine;

public interface IInputProvider
{
    event Action<Vector2> OnClick;
}
