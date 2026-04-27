using UnityEngine;

public static class BoardConverter
{
    private static Vector3 boardOffset = new Vector3(-4, -3, 0);
    private static Vector3 senteHandOffset = new Vector3(-10, -3, 0);
    private static Vector3 goteHandOffset = new Vector3(10, -3, 0);
    private static float cellSize = 1f;
    // -------------------------
    // 盤面用
    // -------------------------
    public static bool WorldToBoard(Vector3 worldPos, out Vector2Int boardPos)
    {
        boardPos = WorldToGrid(worldPos, boardOffset, cellSize);
        return IsInsideBoard(boardPos);
    }

    // -------------------------
    // 先手持ち駒用
    // -------------------------
    public static bool WorldToSenteHand(Vector3 worldPos, out Vector2Int pos)
    {
        pos = WorldToGrid(worldPos, senteHandOffset, cellSize);
        return IsInsideHand(pos);
    }

    // -------------------------
    // 後手持ち駒用
    // -------------------------
    public static bool WorldToGoteHand(Vector3 worldPos, out Vector2Int pos)
    {
        pos = WorldToGrid(worldPos, goteHandOffset, cellSize);
        return IsInsideHand(pos);
    }

    // -------------------------
    // 共通ロジック（本体）
    // -------------------------
    private static Vector2Int WorldToGrid(Vector3 worldPos, Vector3 offset, float cellSize)
    {
        float x = (worldPos.x - offset.x) / cellSize;
        float y = (worldPos.y - offset.y) / cellSize;

        int ix = Mathf.FloorToInt(x);
        int iy = Mathf.FloorToInt(y);

        Vector2Int pos = new Vector2Int(ix, iy);

        return pos; // 必要なら範囲チェックもここに入れる
    }

    public static bool IsInsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x <= 8 &&
            pos.y >= 0 && pos.y <= 8;
    }

    public static bool IsInsideHand(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x <= 1 &&
            pos.y >= 0 && pos.y <= 9;
    }
}
/*
このクラスでは、ボード、先手の持ち駒、後手の持ち駒のための判定を行っている。
まず、選択できる範囲に入っているかどうかをbool値で返し、もし入っているのであればVector2Intで何を選んでいるのかを返す。
IsInsideBoard、IsInsideHandの判定で入力できる範囲の形が確定する。
*/