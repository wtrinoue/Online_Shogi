using UnityEngine;

public class BoardMoveState : IState
{
    public void Enter()
    {
        Debug.Log("BoardMoveStateに入りました、");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if (GameManager.Instance.IsPlaceableOnBoard(boardPos))
            {
                Vector2Int selectedPos = GameManager.Instance.GetSelectedBoardPiecePosition();
                Piece targetPiece = GameManager.Instance.MovePieceFromBoard(selectedPos, boardPos);
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

