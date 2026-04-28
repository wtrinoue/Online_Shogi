using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private GameObject inputObject;
    [SerializeField] private BoardConfig boardConfig;

    private IInputProvider inputAdapter;
    private IState currentState = new IdleState();

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
        currentState.Enter();
    }

    void OnDisable()
    {
        if (inputAdapter != null)
            inputAdapter.OnClickEvent -= OnClick;
    }

    public void ChangeState(IState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

    public void OnClick(Vector2 pos)
    {
        if (currentState == null) return;

        var next = currentState.OnClick(pos);

        if (next != null)
            ChangeState(next);
    }
}