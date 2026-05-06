// using System.Collections;
// using UnityEngine;

// public class NetworkRenderWatcher : MonoBehaviour
// {
//     public NetworkGameManager networkGameManager;

//     private int lastRenderSignal = -1;
//     private Coroutine boostCoroutine;

//     void Update()
//     {
//         if (networkGameManager == null)
//             return;

//         int current = networkGameManager.GetRenderSignal();

//         if (current != lastRenderSignal)
//         {
//             lastRenderSignal = current;

//             // ブースト開始
//             if (boostCoroutine != null)
//                 StopCoroutine(boostCoroutine);

//             boostCoroutine = StartCoroutine(RenderBoost());
//         }
//     }

//     private IEnumerator RenderBoost()
//     {
//         float duration = 0.3f;   // ブースト時間
//         float interval = 0.02f;  // 高頻度描画（約50fps相当）

//         float t = 0f;

//         while (t < duration)
//         {
//             networkGameManager.gameViewer.ReloadAllData();
//             networkGameManager.gameViewer.BuildAll();

//             t += interval;
//             yield return new WaitForSeconds(interval);
//         }

//         boostCoroutine = null;
//     }
// }
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
        {
            Debug.Log("[RenderWatcher] networkGameManager is NULL");
            return;
        }

        int current = networkGameManager.GetRenderSignal();

        // シグナル監視
        if (current != lastRenderSignal)
        {
            Debug.Log($"[RenderWatcher] RenderSignal changed: {lastRenderSignal} -> {current}");

            lastRenderSignal = current;

            // ブースト開始
            if (boostCoroutine != null)
            {
                Debug.Log("[RenderWatcher] Stopping previous boost coroutine");
                StopCoroutine(boostCoroutine);
            }

            Debug.Log("[RenderWatcher] Starting RenderBoost coroutine");
            boostCoroutine = StartCoroutine(RenderBoost());
        }
    }

    private IEnumerator RenderBoost()
    {
        Debug.Log("[RenderBoost] START coroutine");

        float duration = 0.3f;
        float interval = 0.02f;

        float t = 0f;
        int frameCount = 0;

        while (t < duration)
        {
            frameCount++;

            Debug.Log($"[RenderBoost] Frame {frameCount} - Reload/Build start");

            if (networkGameManager != null && networkGameManager.gameViewer != null)
            {
                networkGameManager.gameViewer.ReloadAllData();
                Debug.Log("[RenderBoost] ReloadAllData done");

                networkGameManager.gameViewer.BuildAll();
                Debug.Log("[RenderBoost] BuildAll done");
            }
            else
            {
                Debug.LogWarning("[RenderBoost] gameViewer is NULL");
            }

            t += interval;
            Debug.Log($"[RenderBoost] elapsed time: {t}");

            yield return new WaitForSeconds(interval);
        }

        Debug.Log("[RenderBoost] END coroutine");

        boostCoroutine = null;
    }
}