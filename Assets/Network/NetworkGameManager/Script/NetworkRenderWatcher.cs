using System.Collections;
using UnityEngine;

public class NetworkRenderWatcher : MonoBehaviour
{
    public NetworkGameManager networkGameManager;

    private int lastRenderSignal = -1;
    private Coroutine boostCoroutine;

    void Update()
    {
        if (networkGameManager == null)
            return;

        int current = networkGameManager.GetRenderSignal();

        if (current != lastRenderSignal)
        {
            lastRenderSignal = current;

            // ブースト開始
            if (boostCoroutine != null)
                StopCoroutine(boostCoroutine);

            boostCoroutine = StartCoroutine(RenderBoost());
        }
    }

    private IEnumerator RenderBoost()
    {
        float duration = 0.3f;   // ブースト時間
        float interval = 0.02f;  // 高頻度描画（約50fps相当）

        float t = 0f;

        while (t < duration)
        {
            networkGameManager.gameViewer.ReloadAllData();
            networkGameManager.gameViewer.BuildAll();

            t += interval;
            yield return new WaitForSeconds(interval);
        }

        boostCoroutine = null;
    }
}