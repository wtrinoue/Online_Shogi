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
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}