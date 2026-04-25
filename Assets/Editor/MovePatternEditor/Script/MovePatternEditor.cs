using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(MovePattern))]
public class MovePatternDrawer : PropertyDrawer
{
    private enum CellState
    {
        None,
        Circle,
        Arrow
    }

    private const int SIZE = 5; // ★ 3 → 5に拡張（-2〜2対応）
    private const int OFFSET = 2;

    private CellState[,] grid = new CellState[SIZE, SIZE];
    private bool initialized = false;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 180;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            Load(property);
            initialized = true;
        }

        EditorGUI.LabelField(
            new Rect(position.x, position.y, position.width, 18),
            label
        );

        Rect gridRect = new Rect(position.x, position.y + 20, 140, 140);

        DrawGrid(gridRect);

        if (GUI.changed)
        {
            Apply(property);
        }
    }

    // ----------------------------
    // GRID UI（5x5）
    // ----------------------------
    private void DrawGrid(Rect rect)
    {
        float size = rect.width / SIZE;

        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                Rect r = new Rect(rect.x + x * size, rect.y + y * size, size, size);

                int vx = x - OFFSET;
                int vy = OFFSET - y;

                // 中心
                if (vx == 0 && vy == 0)
                {
                    GUI.Box(r, "■");
                    continue;
                }

                string label = GetLabel(grid[x, y]);

                if (GUI.Button(r, label))
                {
                    Cycle(x, y, vx, vy);
                }
            }
        }
    }

    private string GetLabel(CellState state)
    {
        return state switch
        {
            CellState.None => "",
            CellState.Circle => "○",
            CellState.Arrow => "→",
            _ => ""
        };
    }

    // ----------------------------
    // Cycle（桂馬ゾーンは○のみ）
    // ----------------------------
    private void Cycle(int x, int y, int vx, int vy)
    {
        bool isKnightZone = Mathf.Abs(vy) == 2; // ★ y=±2

        if (isKnightZone)
        {
            // 桂馬ゾーンは Circle固定トグル
            grid[x, y] = grid[x, y] == CellState.Circle ? CellState.None : CellState.Circle;
            return;
        }

        grid[x, y] = (CellState)(((int)grid[x, y] + 1) % 3);
    }

    // ----------------------------
    // LOAD
    // ----------------------------
    private void Load(SerializedProperty property)
    {
        grid = new CellState[SIZE, SIZE];

        var dir = property.FindPropertyRelative("direction");
        var pos = property.FindPropertyRelative("positon");

        // direction → Arrow（y±2は無視）
        for (int i = 0; i < dir.arraySize; i++)
        {
            Vector2Int v = dir.GetArrayElementAtIndex(i).vector2IntValue;

            if (Mathf.Abs(v.y) == 2) continue; // ★桂馬ゾーン除外

            int x = v.x + OFFSET;
            int y = OFFSET - v.y;

            if (!IsValid(x, y)) continue;

            grid[x, y] = CellState.Arrow;
        }

        // position → Circle（y±2対応）
        for (int i = 0; i < pos.arraySize; i++)
        {
            Vector2Int v = pos.GetArrayElementAtIndex(i).vector2IntValue;

            int x = v.x + OFFSET;
            int y = OFFSET - v.y;

            if (!IsValid(x, y)) continue;

            grid[x, y] = CellState.Circle;
        }
    }

    // ----------------------------
    // APPLY
    // ----------------------------
    private void Apply(SerializedProperty property)
    {
        var dir = property.FindPropertyRelative("direction");
        var pos = property.FindPropertyRelative("positon");

        List<Vector2Int> directions = new List<Vector2Int>();
        List<Vector2Int> positions = new List<Vector2Int>();

        dir.ClearArray();
        pos.ClearArray();

        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                int vx = x - OFFSET;
                int vy = OFFSET - y;

                if (vx == 0 && vy == 0) continue;
                if (grid[x, y] == CellState.None) continue;

                Vector2Int v = new Vector2Int(vx, vy);

                // ★桂馬ゾーン（y±2）は Circleのみ
                if (Mathf.Abs(vy) == 2)
                {
                    positions.Add(v);
                    continue;
                }

                if (grid[x, y] == CellState.Arrow)
                {
                    directions.Add(v);
                }
                else if (grid[x, y] == CellState.Circle)
                {
                    positions.Add(v);
                }
            }
        }

        dir.arraySize = directions.Count;
        pos.arraySize = positions.Count;

        for (int i = 0; i < directions.Count; i++)
            dir.GetArrayElementAtIndex(i).vector2IntValue = directions[i];

        for (int i = 0; i < positions.Count; i++)
            pos.GetArrayElementAtIndex(i).vector2IntValue = positions[i];

        property.serializedObject.ApplyModifiedProperties();
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < SIZE && y >= 0 && y < SIZE;
    }
}

