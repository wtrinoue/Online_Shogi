using UnityEngine;
[CreateAssetMenu(menuName = "Asset/PieceData")]
public class PieceData : ScriptableObject
{
    public Sprite normalSprite;
    public Sprite promotedSprite;
    public MovePattern normalMove;
    public MovePattern promotedMove;
}
