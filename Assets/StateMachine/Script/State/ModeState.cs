using UnityEngine;

public class ModeState : State
{
    public ModeState(GameContext context) : base(context)
    {
    }

    public override void Enter()
    {
        Debug.Log("ModeStateに入りました。モード選択してください。");
        switch (context.mode.GetMode())
        {
            case Mode.Local:
                context.machine.ChangeState(
                    new TimerTextState(
                        context,
                        $"{context.turn.GetCurrentTurn()}のターン",
                        1f,
                        new JudgeState(context)
                    )
                );
                break;
            case Mode.Network:
                context.machine.ChangeState(new NetworkJudgeState(context, new NetworkTurnEndState(context)));
                break;
        }
    }

    public override void Exit()
    {
    }

    public override void OnClick(Vector2 pos)
    {
    }
}
