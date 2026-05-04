using UnityEngine;

public class GameContext
{
    public MachineModule machine;
    public ManagerModule manager;
    public ViewerModule viewer;
    public TurnModule turn;
    public TextModule text;
    public ResultModule result;
    public JudgeModule judge;
    public ModeModule mode;

    public GameContext(
        StateMachine stateMachine,
        GameManager gameManager,
        GameViewer gameViewer,
        TextManager textManager,
        Mode initialMode)
    {
        this.machine = new MachineModule(stateMachine);
        this.manager = new ManagerModule(gameManager);
        this.viewer = new ViewerModule(gameViewer);
        this.turn = new TurnModule();
        this.text = new TextModule(textManager);
        this.result = new ResultModule(textManager);
        this.judge = new JudgeModule(gameManager);
        this.mode = new ModeModule(initialMode);
    }
}
