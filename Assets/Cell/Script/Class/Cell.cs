using UnityEngine;

public class Cell
{
    public CellState state;
    public CellColor color;
}

public enum CellState
{
    Normal,     // 何もない通常状態
    Selected,   // 選択されている
    Placeable   // 駒を置ける
}
