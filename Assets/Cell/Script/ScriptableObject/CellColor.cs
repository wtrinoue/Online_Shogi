using UnityEngine;

[CreateAssetMenu(menuName = "Asset/CellColor")]
public class CellColor : ScriptableObject
{
    [Header("Normal State")]
    public Color normalColor;

    [Header("Selected State")]
    public Color selectedColor;

    [Header("Placeable State")]
    public Color placeableColor;
}