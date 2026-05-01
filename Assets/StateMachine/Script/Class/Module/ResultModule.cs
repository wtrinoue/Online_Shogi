public class ResultModule
{
    private TextManager textManager;

    public ResultModule(TextManager textManager)
    {
        this.textManager = textManager;
    }

    // 結果表示
    public void Show(string message)
    {
        textManager.ShowResult(message);
    }

    // 結果非表示
    public void Hide()
    {
        textManager.HideResult();
    }
}