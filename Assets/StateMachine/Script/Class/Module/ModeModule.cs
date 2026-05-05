public class ModeModule
{
    public Mode mode;

    public ModeModule(Mode mode)
    {
        this.mode = mode;
    }

    public Mode GetMode()
    {
        return mode;
    }
}

public enum Mode
{
    Local,
    Network
}