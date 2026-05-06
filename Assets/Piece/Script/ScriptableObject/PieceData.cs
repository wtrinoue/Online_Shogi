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

public enum Team : byte
{
    Sente = 0,
    Gote = 1
}

public enum PieceType : byte
{
    None = 0,
    Fu,      // 歩
    Gin,     // 銀
    Hisha,   // 飛
    Kaku,    // 角
    Kei,     // 桂
    Kin,     // 金
    Kyo,     // 香

    Ou       // 王（またはGyokuでもOK）
}