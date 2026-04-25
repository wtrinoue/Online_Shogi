using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardSetup))]
public class BoardSetupEditor : Editor
{
    private const int SIZE = 9;

    public override void OnInspectorGUI()
    {
        BoardSetup setup = (BoardSetup)target;

        EditorGUILayout.LabelField("Shogi Board Editor (9x9)", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("drag and drop PieceData", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("click to delete", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        DrawGrid(setup);

        EditorGUILayout.Space();

        if (GUILayout.Button("Clear Board"))
        {
            setup.placements.Clear();
            EditorUtility.SetDirty(setup);
        }
    }

    // ----------------------------
    // GRID DRAW
    // ----------------------------
    void DrawGrid(BoardSetup setup)
    {
        for (int y = SIZE - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < SIZE; x++)
            {
                DrawCell(setup, x, y);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    // ----------------------------
    // CELL DRAW
    // ----------------------------
    void DrawCell(BoardSetup setup, int x, int y)
    {
        var cell = GetCell(setup, x, y);

        Rect rect = GUILayoutUtility.GetRect(
            60, 60,
            GUILayout.Width(60),
            GUILayout.Height(60)
        );

        // ----------------------------
        // 背景ボタン（クリック判定だけ）
        // ----------------------------
        if (GUI.Button(rect, GUIContent.none))
        {
            OnCellClicked(setup, x, y);
        }

        // ----------------------------
        // Sprite描画（前面に表示）
        // ----------------------------
        if (cell != null && cell.pieceData != null && cell.pieceData.normalSprite != null)
        {
            Sprite sprite = cell.pieceData.normalSprite;

            Rect r = sprite.textureRect;
            Texture t = sprite.texture;

            Rect uv = new Rect(
                r.x / t.width,
                r.y / t.height,
                r.width / t.width,
                r.height / t.height
            );

            GUI.DrawTextureWithTexCoords(rect, t, uv);
        }

        HandleDragAndDrop(setup, rect, x, y);
    }

    // ----------------------------
    // DRAG & DROP
    // ----------------------------
    void HandleDragAndDrop(BoardSetup setup, Rect rect, int x, int y)
    {
        Event evt = Event.current;

        if (!rect.Contains(evt.mousePosition))
            return;

        if (evt.type == EventType.DragUpdated || evt.type == EventType.DragPerform)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();

                foreach (var obj in DragAndDrop.objectReferences)
                {
                    PieceData piece = obj as PieceData;

                    if (piece != null)
                    {
                        SetPiece(setup, x, y, piece);
                    }
                }
            }

            evt.Use();
        }
    }

    // ----------------------------
    // SET PIECE
    // ----------------------------
    void SetPiece(BoardSetup setup, int x, int y, PieceData pieceData)
    {
        var cell = GetCell(setup, x, y);

        if (cell == null)
        {
            cell = new PiecePlacement
            {
                x = x,
                y = y,
                pieceData = pieceData
            };

            setup.placements.Add(cell);
        }
        else
        {
            cell.pieceData = pieceData;
        }

        EditorUtility.SetDirty(setup);
    }

    // ----------------------------
    // CLICK (DELETE)
    // ----------------------------
    void OnCellClicked(BoardSetup setup, int x, int y)
    {
        var cell = GetCell(setup, x, y);

        if (cell != null)
        {
            setup.placements.Remove(cell);
            EditorUtility.SetDirty(setup);
        }
    }

    // ----------------------------
    // GET CELL
    // ----------------------------
    PiecePlacement GetCell(BoardSetup setup, int x, int y)
    {
        return setup.placements.Find(p => p.x == x && p.y == y);
    }
}