using UnityEngine;
[CreateAssetMenu(menuName = "Asset/PieceData")]
public class PieceData : ScriptableObject
{
    public Team team;
    public PieceType type;
    public Sprite normalSprite;
    public Sprite promotedSprite;
    public MovePattern normalMove;
    public MovePattern promotedMove;
}

public enum Team
{
    Sente,
    Gote
}

public enum PieceType
{
    Fu,      // 歩
    Kyo,     // 香
    Kei,     // 桂
    Gin,     // 銀
    Kin,     // 金
    Kaku,    // 角
    Hisha,   // 飛
    Ou       // 王（またはGyokuでもOK）
}