using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public GameManager gameManager;
    public GameViewer gameViewer;
    public StateMachine stateMachine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.Init();
        gameViewer.Init();
        stateMachine.Init();
    }
}
