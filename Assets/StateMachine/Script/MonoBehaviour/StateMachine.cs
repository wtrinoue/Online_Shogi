using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private GameObject inputObject;

    private IInputProvider inputAdapter;
    private IState currentState = new TestState();

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
        inputAdapter.OnClickEvent += OnClick;
    }

    void OnDisable()
    {
        if (inputAdapter != null)
            inputAdapter.OnClickEvent -= OnClick;
    }

    public void ChangeState(IState state)
    {
        currentState = state;
    }

    public void OnClick(Vector2 pos)
    {
        if (currentState == null) return;

        var next = currentState.OnClick(pos);

        if (next != null)
            ChangeState(next);
    }
}