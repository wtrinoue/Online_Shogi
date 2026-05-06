using UnityEngine;

public class NetworkRenderWatcher : MonoBehaviour
{
    private NetworkGameManager networkGameManager;

    private int lastRenderSignal = -1;
    private bool needsRender;

    void Awake()
    {
        networkGameManager = GetComponentInParent<NetworkGameManager>();
    }
    void Update()
    {
        if (networkGameManager == null)
            return;

        int current = networkGameManager.GetRenderSignal();

        // 変化検知
        if (current != lastRenderSignal)
        {
            lastRenderSignal = current;
            needsRender = true;
        }

        // 描画実行（Update側でやるならここでOK）
        if (needsRender)
        {
            needsRender = false;

            networkGameManager.gameViewer.ReloadAllData();
            networkGameManager.gameViewer.BuildAll();

            Debug.Log("NetworkRenderSupportにより再描画");
        }
    }
}