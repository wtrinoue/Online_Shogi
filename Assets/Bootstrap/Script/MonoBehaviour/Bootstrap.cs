using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GameObject gameManagerObject;
    public GameViewer gameViewer;
    public StateMachine stateMachine;
    private IGameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }
    public void SetGameManager(IGameManager gm)
    {
        gameManager = gm;
    }
    public void Init()
    {
        if(gameManagerObject != null)
        {
            gameManager = gameManagerObject.GetComponent<IGameManager>();
        }
        gameManager.Init();
        gameViewer.Init();
        stateMachine.Init();
    }
}
