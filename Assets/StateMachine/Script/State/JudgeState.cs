using UnityEngine;

public class JudgeState : State
{
    public JudgeState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("JudgeStateに入りました");
        if(StateModule.Judge.IsEnd(out Team winner))
        {
            stateMachine.ChangeState(new EndState(stateMachine, winner));
        }
        else
        {
            stateMachine.ChangeState(
                    new TimerTextState(
                        stateMachine,
                        $"{StateModule.Turn.GetCurrentTurn()}のターン",
                        1f,
                        new IdleState(stateMachine)
                    )
            );
        }
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}