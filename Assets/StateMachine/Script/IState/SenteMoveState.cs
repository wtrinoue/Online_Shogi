using UnityEngine;

public class SenteMoveState : IState
{
    public void Enter()
    {
        Debug.Log("SenteMoveStateに入りました");
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        if(BoardConverter.WorldToBoard(pos, out Vector2Int boardPos))
        {
            if (GameManager.Instance.IsPlaceableOnBoard(boardPos))
            {
                Vector2Int selectedPos = GameManager.Instance.GetSelectedSenteHandPiecePosition();
                Piece targetPiece = GameManager.Instance.MovePieceFromSenteHand(selectedPos, boardPos);
                if(targetPiece != null)
                {
                    GameManager.Instance.AddToHand(targetPiece);
                }
                if (GameManager.Instance.IsPromotable(boardPos))
                {
                    GameManager.Instance.PromotePiece(boardPos);
                }
                return new IdleState();
            }
            return null;
        }
        return new SelectState();
    }
}
