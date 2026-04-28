using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        Debug.Log("IdleStateに入りました");
        GameManager.Instance.ClearCells();
        GameViewer.Instance.ReloadAllData();
        GameViewer.Instance.BuildAll();
    }
    public void Exit(){}
    public IState OnClick(Vector2 pos){
        return new TextState("駒を選んでください",new SelectState());
    }
}
