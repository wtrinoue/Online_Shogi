using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using System.Linq;

public class NetworkGameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner runnerPrefab;
    [SerializeField] private NetworkPrefabRef networkGameManagerPrefab;
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private GameViewer gameViewer;
    [SerializeField] private TextManager textManager;

    private NetworkRunner runner;
    private NetworkGameManager networkGameManager;
    private bool isInitialized = false;
    private void Awake()
    {
        textManager.ShowResult("相手を待っています...");
    }
    private async void Start()
    {
        runner = Instantiate(runnerPrefab);
        runner.AddCallbacks(this);
        runner.ProvideInput = true;

        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            PlayerCount = 2
        });
    }

     void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
     void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
    // =========================
    // 参加時：ゲームマネージャをホストで生成し、2人揃ったら開始
    // =========================
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        int count = runner.ActivePlayers.Count();

        if (count == 1)
        {
            Debug.Log("一番目に実行しました（Sente確定）");
            OnFirstPlayerJoined(runner, player);
        }
        else if (count == 2)
        {
            Debug.Log("二番目に実行しました（Gote確定）");
            OnSecondPlayerJoined(runner, player);
        }
    }

    public void OnFirstPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        var net = runner.Spawn(networkGameManagerPrefab, Vector3.zero, Quaternion.identity, player);
        networkGameManager = net.GetComponent<NetworkGameManager>();
        networkGameManager.SentePlayer = player;
    }

    public void OnSecondPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == networkGameManager.SentePlayer)
        {
            networkGameManager.GotePlayer = player;
            StartCoroutine(WaitForGameStart(true));
        }
        else
        {
            StartCoroutine(WaitForGameStart(false));
        }
    }

    private System.Collections.IEnumerator WaitForGameStart(bool isHost)
    {
        while (networkGameManager == null)
        {
            networkGameManager = FindObjectOfType<NetworkGameManager>();
            if (networkGameManager != null)
                break;
            yield return null;
        }
        textManager.HideResult();
        textManager.ShowMessage("マッチングしました！");
        yield return new WaitForSeconds(3f);
        textManager.HideMessage();

        if (isHost)
        {
            networkGameManager.Init();
            stateMachine.SenteInit();
        }
        else
        {
            stateMachine.GoteInit();
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("プレイヤーが退出");

        if (runner.IsServer)
        {
            // 相手が消えた → 勝利扱いなど
        }
    }

    // =========================
    // 入力送信
    // =========================
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input){}

     void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
     void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {}
     void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) {}
     void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("切断されました: " + reason);

        // UI表示やState変更
    }
     void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {}
     void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {}
     void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {}
     void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {}
     void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {}
     void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {}
     void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) {}
     void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) {}
     void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) {}
     void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) {}
}