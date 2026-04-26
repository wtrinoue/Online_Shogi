using UnityEngine;

public class Cell
{
    public CellState state;
    // コンストラクタ
    public Cell(CellState state)
    {
        this.state = state;
    }

    // デフォルトコンストラクタ（必要なら）
    public Cell()
    {
        state = CellState.Normal;
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
