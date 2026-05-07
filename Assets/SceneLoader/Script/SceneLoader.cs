using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum SceneType
    {
        Title,
        LocalGame,
        NetworkGame
    }

    private Dictionary<SceneType, string> sceneMap = new()
    {
        { SceneType.Title, "TitleScene" },
        { SceneType.LocalGame, "LocalGameScene" },
        { SceneType.NetworkGame, "NetworkGameScene"}
    };

    [SerializeField] private SceneType sceneType;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneMap[sceneType]);
    }
}