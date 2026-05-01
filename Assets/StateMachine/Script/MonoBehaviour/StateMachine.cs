using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private GameObject inputObject;
    [SerializeField] private BoardConfig boardConfig;

    private IInputProvider inputAdapter;
    private State currentState;
    public GameContext context;

    void Awake()
    {
        inputAdapter = inputObject.GetComponent<IInputProvider>();

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
        StateModule.Turn.SetTurn(Team.Sente);
        currentState = new TimerTextState(this, $"{StateModule.Turn.GetCurrentTurn()}のターン", 1f, new IdleState(this));
        currentState.Enter();
    }
    void OnDisable()
    {
        if (inputAdapter != null)
            inputAdapter.OnClickEvent -= OnClick;
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