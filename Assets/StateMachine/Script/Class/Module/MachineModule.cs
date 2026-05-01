using System.Collections;
public class MachineModule
{
    private StateMachine stateMachine;

    public MachineModule(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void ChangeState(State state)
    {
        stateMachine.ChangeState(state);
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        stateMachine.RunCoroutine(coroutine);
    }
}