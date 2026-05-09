using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private GameObject inputObject;
    [SerializeField] private BoardConfig boardConfig;
    [SerializeField] private GameObject gameManagerObject;
    [SerializeField] private GameViewer gameViewer;
    [SerializeField] private TextManager textManager;
    [SerializeField] private Mode mode;
    private IGameManager gameManager;

    private IInputProvider inputAdapter;
    private State currentState ;
    public GameContext context;

    void Awake()
    {
        inputAdapter = inputObject.GetComponent<IInputProvider>();
        gameManager = gameManagerObject.GetComponent<IGameManager>();
        currentState = new EmptyState(context);
        if (inputAdapter == null)
        {
            Debug.LogError("IInputProviderが見つかりません");
            return;
        }
    }

    void Start()
    {
        BoardConverter.SetBoardConfig(boardConfig);
        inputAdapter.OnClickEvent += OnClick;
    }
    public void Init()
    {
        context = new GameContext(this, gameManager, gameViewer, textManager, mode);
        currentState = new SenteEntryState(context);
        currentState.Enter();
    }
    public void SenteInit()
    {
        context = new GameContext(this, gameManager, gameViewer, textManager, mode);
        currentState = new SenteEntryState(context);
        currentState.Enter(); 
    }

    public void GoteInit()
    {
        context = new GameContext(this, gameManager, gameViewer, textManager, mode);
        currentState = new TimerTextState(context, $"{context.turn.GetCurrentTurn()}のターン", 1f, new GoteEntryState(context));
        currentState.Enter();
    }
    void OnDisable()
    {
        if (inputAdapter != null)
            inputAdapter.OnClickEvent -= OnClick;
    }

    public void SetGameManager(IGameManager gm)
    {
        gameManager = gm;
    }

    public void ChangeState(State state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    public void OnClick(Vector2 pos)
    {
        currentState.OnClick(pos);
    }

    public void RunCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}