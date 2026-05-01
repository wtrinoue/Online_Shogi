public class ViewerModule
{
    private GameViewer viewer;

    public ViewerModule(GameViewer viewer)
    {
        this.viewer = viewer;
    }

    // =========================
    // ビルド系
    // =========================

    public void BuildBoard()
    {
        viewer.ReloadCellBoard();
        viewer.BuildCellBoard();
    }

    public void BuildSenteHand()
    {
        viewer.ReloadSenteHandCells();
        viewer.BuildSenteHandCells();
    }

    public void BuildGoteHand()
    {
        viewer.ReloadGoteHandCells();
        viewer.BuildGoteHandCells();
    }

    public void BuildAll()
    {
        viewer.ReloadAllData();
        viewer.BuildAll();
    }
}