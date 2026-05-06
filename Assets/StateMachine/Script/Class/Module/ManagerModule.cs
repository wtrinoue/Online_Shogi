using UnityEngine;
using System.Collections.Generic;

public class ManagerModule
{
    private IGameManager gm;

    public ManagerModule(IGameManager gm)
    {
        this.gm = gm;
    }

    // =========================
    // 選択系
    // =========================

    public Team GetSelectedPieceTeam()
    {
        return gm.GetSelectedPiece().data.team;
    }

    public bool SelectBoardPiece(Vector2Int pos)
    {
        Piece piece = gm.GetBoardPiece(pos);
        if (piece == null) return false;

        gm.SetSelectedBoardPiecePosition(pos);
        return true;
    }

    public bool SelectSentePiece(Vector2Int pos)
    {
        Piece piece = gm.GetSenteHandPiece(pos);
        if (piece == null) return false;

        gm.SetSelectedSenteHandPiecePosition(pos);
        return true;
    }

    public bool SelectGotePiece(Vector2Int pos)
    {
        Piece piece = gm.GetGoteHandPiece(pos);
        if (piece == null) return false;

        gm.SetSelectedGoteHandPiecePosition(pos);
        return true;
    }

    // =========================
    // 移動系
    // =========================

    public void MoveFromBoard(Vector2Int pos)
    {
        Vector2Int selectedPos = gm.GetSelectedBoardPiecePosition();

        Piece target = gm.MovePieceFromBoard(selectedPos, pos);
        if (target != null)
        {
            gm.AddToHand(target);
        }

        if (gm.IsPromotable(pos))
        {
            gm.PromotePiece(pos);
        }
    }

    public void MoveFromSenteHand(Vector2Int pos)
    {
        Vector2Int selectedPos = gm.GetSelectedSenteHandPiecePosition();

        Piece target = gm.MovePieceFromSenteHand(selectedPos, pos);
        if (target != null)
        {
            gm.AddToHand(target);
        }

        if (gm.IsPromotable(pos))
        {
            gm.PromotePiece(pos);
        }
    }

    public void MoveFromGoteHand(Vector2Int pos)
    {
        Vector2Int selectedPos = gm.GetSelectedGoteHandPiecePosition();

        Piece target = gm.MovePieceFromGoteHand(selectedPos, pos);
        if (target != null)
        {
            gm.AddToHand(target);
        }

        if (gm.IsPromotable(pos))
        {
            gm.PromotePiece(pos);
        }
    }

    // =========================
    // 判定系
    // =========================

    public bool IsPlaceable(Vector2Int pos)
    {
        return gm.IsPlaceableOnBoard(pos);
    }

    // =========================
    // セル制御（ハイライト）
    // =========================

    public void ChangeCellsByBoardPiece(Vector2Int pos)
    {
        gm.ClearCells();
        gm.ChangeBoardCellSelected(pos);

        List<Vector2Int> positions = gm.GetBoardPieceMovablePositions(pos);
        gm.ChangeBoardCells(positions);
    }

    public void ChangeCellsBySenteHandPiece(Vector2Int pos)
    {
        gm.ClearCells();
        gm.ChangeSenteHandCellSelected(pos);

        List<Vector2Int> positions = gm.GetHandPieceMovablePositions();
        gm.ChangeBoardCells(positions);
    }

    public void ChangeCellsByGoteHandPiece(Vector2Int pos)
    {
        gm.ClearCells();
        gm.ChangeGoteHandCellSelected(pos);

        List<Vector2Int> positions = gm.GetHandPieceMovablePositions();
        gm.ChangeBoardCells(positions);
    }

    public void ClearCells()
    {
        gm.ClearCells();
    }
    // =========================
    // ネットワーク対戦用
    // =========================
    public void ChangeIsMovedTo(bool isMoved){
        gm.ChangeIsMovedTo(isMoved);
    }

    public int GetMoveSignal()
    {
        return gm.GetMoveSignal();
    }

    public void SignalMove(Team movedTeam)
    {
        gm.SignalMove(movedTeam);
    }

    public Team GetLastMovedTeam()
    {
        return gm.GetLastMovedTeam();
    }

    public bool GetIsMoved(){
        return gm.GetIsMoved();
    }
}
