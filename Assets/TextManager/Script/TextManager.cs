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