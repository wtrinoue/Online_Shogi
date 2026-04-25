using UnityEngine;

public class PieceView : MonoBehaviour
{
    public SpriteRenderer sr;
    public Piece piece;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SetPiece(Piece p)
    {
        piece = p;
        UpdateView();
    }
    // Update is called once per frame
    public void UpdateView()
    {
        if (piece.isPromoted)
            sr.sprite = piece.data.promotedSprite;
        else
            sr.sprite = piece.data.normalSprite;
    }
}
