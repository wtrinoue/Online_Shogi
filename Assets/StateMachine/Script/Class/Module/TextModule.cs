public class TextModule
{
    private TextManager textManager;

    public TextModule(TextManager textManager)
    {
        this.textManager = textManager;
    }

    // メッセージ表示
    public void Show(string message)
    {
        textManager.ShowMessage(message);
    }

    // メッセージ非表示
    public void Hide()
    {
        textManager.HideMessage();
    }
}