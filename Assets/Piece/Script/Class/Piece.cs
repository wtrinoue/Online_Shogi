using UnityEngine;
public class Piece
{
    public PieceData data;
    public bool isPromoted;

    public MovePattern GetMove()
    {
        return isPromoted
            ? data.promotedMove
            : data.normalMove;
    }
}
