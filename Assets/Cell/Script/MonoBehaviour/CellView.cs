using UnityEngine;

public class CellView : MonoBehaviour
{
    public SpriteRenderer sr;
    public Cell cell;
    public CellColor cellColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetCell(Cell c)
    {
        cell = c;
    }

    // Update is called once per frame
    public void UpdateView()
    {
        switch (cell.state)
        {
            case CellState.Normal:
                sr.color = cellColor.normalColor;
                break;

            case CellState.Selected:
                sr.color = cellColor.selectedColor;
                break;

            case CellState.Placeable:
                sr.color = cellColor.placeableColor;
                break;
        }
    }
}
