public class JudgeModule
{
    private IGameManager gm;

    public JudgeModule(IGameManager gm)
    {
        this.gm = gm;
    }

    // ゲーム終了判定
    public bool IsEnd(out Team winner)
    {
        return gm.IsGameOver(out winner);
    }
}