using UnityEngine;

public class Cell
{
    public CellState state;
    public CellColor color;
    // コンストラクタ
    public Cell(CellState state, CellColor color)
    {
        this.state = state;
        this.color = color;
    }

    // デフォルトコンストラクタ（必要なら）
    public Cell()
    {
        state = CellState.Normal;
        color = null;
    }

    public void SetNormal()
    {
        state = CellState.Normal;
    }
    public void SetSelected()
    {
        state = CellState.Selected;
    }
    public void SetPlaceable()
    {
        state = CellState.Placeable;
    }
}

public enum CellState
{
    Normal,     // 何もない通常状態
    Selected,   // 選択されている
    Placeable   // 駒を置ける
}
