using UnityEngine;
public class EndState : State
{
    private Team winner;

    public EndState(StateMachine stateMachine, Team winner) : base(stateMachine)
    {
        this.winner = winner;
    }

    public override void Enter()
    {
        Debug.Log($"EndStateに入りました。勝者: {winner}");
        StateModule.Viewer.BuildAll();
        StateModule.Result.Show($"{winner}の勝ち！");
    }

    public override void Exit()
    {
        StateModule.Result.Hide();
    }

    public override void OnClick(Vector2 pos)
    {
    }
}