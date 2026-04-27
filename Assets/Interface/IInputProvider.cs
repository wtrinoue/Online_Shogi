using UnityEngine;
using System;

public interface IInputProvider
{
    event Action<Vector2> OnClickEvent;
}
