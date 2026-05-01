using UnityEngine;
public class EndState : State
{
    private Team winner;

    public EndState(GameContext context, Team winner) : base(context)
    {
        this.winner = winner;
    }

    public override void Enter()
    {
        Debug.Log($"EndStateに入りました。勝者: {winner}");
        context.viewer.BuildAll();
        context.result.Show($"{winner}の勝ち！");
    }

    public override void Exit()
    {
        context.result.Hide();
    }

    public override void OnClick(Vector2 pos)
    {
    }
}