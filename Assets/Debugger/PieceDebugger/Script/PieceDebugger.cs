using UnityEngine;

public class PieceDebugger : MonoBehaviour
{
    [SerializeField] private PieceData data;

    void Start()
    {
        Debug.Log(BuildDebugText());
    }

    private string BuildDebugText()
    {
        if (data == null)
        {
            return "===== PIECE DATA =====\nDATA IS NULL";
        }

        string text =
            "===== PIECE DATA =====\n" +
            $"Team: {data.team}\n" +
            $"Type: {data.type}\n\n" +

            "--- NORMAL SPRITE ---\n" +
            $"Sprite: {(data.normalSprite != null ? data.normalSprite.name : "null")}\n\n" +

            "--- PROMOTED SPRITE ---\n" +
            $"Sprite: {(data.promotedSprite != null ? data.promotedSprite.name : "null")}\n\n" +

            "--- NORMAL MOVE ---\n" +
            FormatMovePattern(data.normalMove) +

            "\n--- PROMOTED MOVE ---\n" +
            FormatMovePattern(data.promotedMove);

        return text;
    }

    private string FormatMovePattern(MovePattern move)
    {
        if (move == null)
            return "null\n";

        string result = "";

        // --- direction ---
        result += "Directions:\n";

        if (move.direction != null && move.direction.Length > 0)
        {
            foreach (var d in move.direction)
            {
                result += $"  (x:{d.x}, y:{d.y})\n";
            }
        }
        else
        {
            result += "  none\n";
        }

        result += "\n";

        // --- position ---
        result += "Positions:\n";

        if (move.positon != null && move.positon.Length > 0)
        {
            foreach (var p in move.positon)
            {
                result += $"  (x:{p.x}, y:{p.y})\n";
            }
        }
        else
        {
            result += "  none\n";
        }

        return result;
    }
}