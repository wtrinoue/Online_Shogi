using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance;

    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Show("実験");
        Hide();
    }

    public void Show(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
    }

    public void Hide()
    {
        messagePanel.SetActive(false);
    }
}