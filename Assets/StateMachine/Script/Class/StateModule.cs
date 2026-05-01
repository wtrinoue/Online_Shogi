using UnityEngine;
using System.Collections.Generic;
public static class StateModule
{
    public static class Manager
    {
        public static Team GetSelectedPieceTeam()
        {
            return GameManager.Instance.GetSelectedPiece().data.team;
        }
        public static bool SelectBoardPiece(Vector2Int pos)
        {
            Piece piece = GameManager.Instance.GetBoardPiece(pos);
            if(piece != null){
                GameManager.Instance.SetSelectedBoardPiecePosition(pos);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SelectSentePiece(Vector2Int pos)
        {
            Piece piece = GameManager.Instance.GetSenteHandPiece(pos);
            if(piece != null)
            {
                GameManager.Instance.SetSelectedSenteHandPiecePosition(pos);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SelectGotePiece(Vector2Int pos)
        {
            Piece piece = GameManager.Instance.GetGoteHandPiece(pos);
            if(piece != null)
            {
                GameManager.Instance.SetSelectedGoteHandPiecePosition(pos);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPlaceable(Vector2Int pos)
        {
            return GameManager.Instance.IsPlaceableOnBoard(pos);
        }
        public static void MoveFromBoard(Vector2Int pos)
        {
            Vector2Int selectedPos = GameManager.Instance.GetSelectedBoardPiecePosition();
            Piece targetPiece = GameManager.Instance.MovePieceFromBoard(selectedPos, pos);
            if(targetPiece != null)
            {
                GameManager.Instance.AddToHand(targetPiece);
            }
            if(GameManager.Instance.IsPromotable(pos))
            {
                GameManager.Instance.PromotePiece(pos);
            }
        }
        public static void MoveFromSenteHand(Vector2Int pos)
        {
            Vector2Int selectedPos = GameManager.Instance.GetSelectedSenteHandPiecePosition();
            Piece targetPiece = GameManager.Instance.MovePieceFromSenteHand(selectedPos, pos);
            if(targetPiece != null)
            {
                GameManager.Instance.AddToHand(targetPiece);
            }
            if (GameManager.Instance.IsPromotable(pos))
            {
                GameManager.Instance.PromotePiece(pos);
            }
        }

        public static void MoveFromGoteHand(Vector2Int pos)
        {
            Vector2Int selectedPos = GameManager.Instance.GetSelectedGoteHandPiecePosition();
            Piece targetPiece = GameManager.Instance.MovePieceFromGoteHand(selectedPos, pos);
            if(targetPiece != null)
            {
                GameManager.Instance.AddToHand(targetPiece);
            }
            if (GameManager.Instance.IsPromotable(pos))
            {
                GameManager.Instance.PromotePiece(pos);
            }
        }

        public static void ChangeCellsByBoardPiece(Vector2Int pos)
        {
            GameManager.Instance.ClearCells();
            GameManager.Instance.ChangeBoardCellSelected(pos);
            List<Vector2Int> positions = GameManager.Instance.GetBoardPieceMovablePositions(pos);
            GameManager.Instance.ChangeBoardCells(positions);
        }

        public static void ChangeCellsBySenteHandPiece(Vector2Int pos)
        {
            GameManager.Instance.ClearCells();
            GameManager.Instance.ChangeSenteHandCellSelected(pos);
            List<Vector2Int> positions = GameManager.Instance.GetHandPieceMovablePositions();
            GameManager.Instance.ChangeBoardCells(positions);
        }

        public static void ChangeCellsByGoteHandPiece(Vector2Int pos)
        {
            GameManager.Instance.ClearCells();
            GameManager.Instance.ChangeGoteHandCellSelected(pos);
            List<Vector2Int> positions = GameManager.Instance.GetHandPieceMovablePositions();
            GameManager.Instance.ChangeBoardCells(positions);
        }
        public static void ClearCells()
        {
            GameManager.Instance.ClearCells();
        }
    }
    public static class Viewer
    {
        public static void BuildBoard()
        {
            GameViewer.Instance.ReloadCellBoard();
            GameViewer.Instance.BuildCellBoard();
        }

        public static void BuildSenteHand()
        {
            GameViewer.Instance.ReloadSenteHandCells();
            GameViewer.Instance.BuildSenteHandCells();
        }

        public static void BuildGoteHand()
        {
            GameViewer.Instance.ReloadGoteHandCells();
            GameViewer.Instance.BuildGoteHandCells();
        }

        public static void BuildAll()
        {
            GameViewer.Instance.ReloadAllData();
            GameViewer.Instance.BuildAll();
        }
    }
    public static class Turn
    {
        private static Team currentTurn = Team.Sente;
        public static void ChangeTurn()
        {
            currentTurn = currentTurn == Team.Sente ? Team.Gote : Team.Sente;
        }
        public static Team GetCurrentTurn()
        {
            return currentTurn;
        }
    }

    public static class Text
    {
        public static void Show(string message)
        {
            TextManager.Instance.Show(message);
        }

        public static void Hide()
        {
            TextManager.Instance.Hide();
        }
    }

    public static class Judge
    {
        public static bool IsGameOver(out Team winner)
        {
            if(GameManager.Instance.IsGameOver(out Team win))
            {
                winner = win;
                return true;
            }
            winner = Team.Sente; // ダミーの値
            return false;
        }
    }
}

//ネストクラスで、Manager、Viewer、Turnに分けるとわかりやすいかも。