using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SceneType
    {
        Title,
        LocalGame,
        Result
    }

    private Dictionary<SceneType, string> sceneMap = new()
    {
        { SceneType.Title, "TitleScene" },
        { SceneType.LocalGame, "LocalGameScene" },
    };

    [SerializeField] private SceneType sceneType;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneMap[sceneType]);
    }
}