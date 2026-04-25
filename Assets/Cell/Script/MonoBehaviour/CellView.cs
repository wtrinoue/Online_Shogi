using UnityEngine;

public class CellView : MonoBehaviour
{
    public SpriteRenderer sr;
    public Cell cell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void SetCell(Cell c)
    {
        cell = c;
    }

    // Update is called once per frame
    void UpdateView()
    {
        switch (cell.state)
        {
            case CellState.Normal:
                sr.color = cell.color.normalColor;
                break;

            case CellState.Selected:
                sr.color = cell.color.selectedColor;
                break;

            case CellState.Placeable:
                sr.color = cell.color.placeableColor;
                break;
        }
    }
}
