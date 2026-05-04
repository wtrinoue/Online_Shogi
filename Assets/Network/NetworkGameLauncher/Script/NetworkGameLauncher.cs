using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class NetworkGameLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkRunner runnerPrefab;
    [SerializeField] private NetworkPrefabRef networkGameManagerPrefab;
    [SerializeField] private Bootstrap bootstrap;
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private GameViewer gameViewer;

    private NetworkRunner runner;
    private NetworkGameManager networkGameManager;

    private async void Start()
    {
        runner = Instantiate(runnerPrefab);
        runner.AddCallbacks(this);
        runner.ProvideInput = true;

        await runner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
        });
    }

     void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
     void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {}
    // =========================
    // 参加時：カウンター生成（1つだけ）
    // =========================
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (!runner.IsServer) return;
        if (FindObjectOfType<NetworkGameManager>() == null)
        {
            var net = runner.Spawn(networkGameManagerPrefab, Vector3.zero, Quaternion.identity);
            networkGameManager = net.GetComponent<NetworkGameManager>();
            IGameManager gm = networkGameManager.GetComponent<IGameManager>();
            stateMachine.SetGameManager(gm);
            gameViewer.SetGameManager(gm);
            bootstrap.SetGameManager(gm);
            bootstrap.Init();
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {}

    // =========================
    // 入力送信
    // =========================
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input){}

     void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
     void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {}
     void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) {}
     void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {}
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