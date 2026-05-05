using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{

    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageText;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;

    private void Awake()
    {
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
    }

    public void HideMessage()
    {
        messagePanel.SetActive(false);
    }

    public void ShowResult(string message)
    {
        resultText.text = message;
        resultPanel.SetActive(true);
    }

    public void HideResult()
    {
        resultPanel.SetActive(false);
    }
}