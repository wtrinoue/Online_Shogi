using UnityEngine;

public class GoteMoveState : IState
{
    public void Enter()
    {
        Debug.Log("GoteMoveStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if (GameManager.Instance.IsPlaceableOnBoard(boardPos))
            {
                Vector2Int selectedPos = GameManager.Instance.GetSelectedGoteHandPiecePosition();
                Piece targetPiece = GameManager.Instance.MovePieceFromGoteHand(selectedPos, boardPos);
                if(targetPiece != null)
                {
                    GameManager.Instance.AddToHand(targetPiece);
                }
                if(GameManager.Instance.IsPromotable(boardPos))
                {
                    GameManager.Instance.PromotePiece(boardPos);
                }
                return new IdleState();
            }
            return new SelectState();
        }
        return new SelectState();
    }
}
