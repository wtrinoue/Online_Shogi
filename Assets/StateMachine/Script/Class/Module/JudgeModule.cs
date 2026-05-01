public class JudgeModule
{
    private GameManager gm;

    public JudgeModule(GameManager gm)
    {
        this.gm = gm;
    }

    // ゲーム終了判定
    public bool IsEnd(out Team winner)
    {
        return gm.IsGameOver(out winner);
    }
}