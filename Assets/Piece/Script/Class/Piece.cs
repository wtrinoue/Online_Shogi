using UnityEngine;
public enum Team
{
    Sente,
    Gote
}
public class Piece
{
    public PieceData data;
    public Team team;
    public bool isPromoted;

    public MovePattern GetMove()
    {
        return isPromoted
            ? data.promotedMove
            : data.normalMove;
    }
}
