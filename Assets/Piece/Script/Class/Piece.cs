using UnityEngine;
public class Piece
{
    public PieceData data;
    public bool isPromoted;
    public bool isHanded;

    public Piece(PieceData data, bool isPromoted = false, bool isHanded = false)
    {
        this.data = data;
        this.isPromoted = isPromoted;
        this.isHanded = isHanded;
    }
    public MovePattern GetMovePattern()
    {
        return isPromoted
            ? data.promotedMove
            : data.normalMove;
    }

    public void Promote()
    {
        if(!isHanded)return;
        this.isPromoted = true;
    }
}
