using UnityEngine;
public class TurnModule
{
    private Team currentTurn = Team.Sente;

    // 現在のターン取得
    public Team GetCurrentTurn()
    {
        return currentTurn;
    }

    // ターン変更
    public void ChangeTurn()
    {
        Debug.Log("Turnを変更しました");
        currentTurn = currentTurn == Team.Sente
            ? Team.Gote
            : Team.Sente;
    }

    // 明示的にセット（リプレイ・同期用など）
    public void SetTurn(Team turn)
    {
        currentTurn = turn;
    }
}