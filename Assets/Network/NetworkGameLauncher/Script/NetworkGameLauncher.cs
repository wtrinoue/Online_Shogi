using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkGameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner runnerPrefab;
    [SerializeField] private NetworkPrefabRef networkGameManagerPrefab;
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private GameViewer gameViewer;
    [SerializeField] private TextManager textManager;
    [SerializeField] private string sessionName = "OnlineShogi";

    private NetworkRunner runner;
    private NetworkGameManager networkGameManager;
    private Coroutine waitForGameStartCoroutine;
    private bool isInitialized;

    private void Awake()
    {
        textManager.ShowResult("相手を探しています...");
    }

    private async void Start()
    {
        runner = Instantiate(runnerPrefab);
        runner.AddCallbacks(this);
        runner.ProvideInput = true;

        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.Shared,
            SessionName = sessionName,
            PlayerCount = 2,
            IsOpen = true,
            IsVisible = true
        });
    }
    private async void OnDestroy()
    {
        if (runner != null)
        {
            runner.RemoveCallbacks(this);

            await runner.Shutdown();

            Destroy(runner.gameObject);

            runner = null;
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.ActivePlayers.Count() < 2)
        {
            return;
        }

        if (runner.IsSharedModeMasterClient && networkGameManager == null)
        {
            SpawnGameManager(runner);
        }

        StartWaitingForGameStart();
    }

    private void SpawnGameManager(NetworkRunner runner)
    {
        var net = runner.Spawn(
            networkGameManagerPrefab,
            Vector3.zero,
            Quaternion.identity,
            inputAuthority: null,
            onBeforeSpawned: null,
            flags: NetworkSpawnFlags.SharedModeStateAuthMasterClient);

        networkGameManager = net.GetComponent<NetworkGameManager>();
        var players = runner.ActivePlayers.OrderBy(p => p.PlayerId).ToArray();
        networkGameManager.SentePlayer = players[0];
        networkGameManager.GotePlayer = players[1];
    }

    private void StartWaitingForGameStart()
    {
        if (waitForGameStartCoroutine != null)
        {
            return;
        }

        waitForGameStartCoroutine = StartCoroutine(WaitForGameStart());
    }

    private IEnumerator WaitForGameStart()
    {
        while (networkGameManager == null)
        {
            networkGameManager = FindObjectOfType<NetworkGameManager>();
            yield return null;
        }

        while (networkGameManager.SentePlayer == PlayerRef.None ||
               networkGameManager.GotePlayer == PlayerRef.None)
        {
            yield return null;
        }

        if (isInitialized)
        {
            yield break;
        }

        isInitialized = true;

        textManager.HideResult();
        textManager.ShowMessage("マッチングしました");
        yield return new WaitForSeconds(3f);
        textManager.HideMessage();

        if (networkGameManager.Object.HasStateAuthority)
        {
            networkGameManager.Init();
        }

        if (runner.LocalPlayer == networkGameManager.SentePlayer)
        {
            stateMachine.SenteInit();
        }
        else if (runner.LocalPlayer == networkGameManager.GotePlayer)
        {
            stateMachine.GoteInit();
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("プレイヤーが退出しました");
        textManager.ShowResult("相手が退出しました...");
    }

    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) {}
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        textManager.ShowResult("相手が退出しました...");
    }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) {}
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        Debug.Log("切断されました: " + reason);
        textManager.ShowResult("相手が退出しました...");
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
    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
}
