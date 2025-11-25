using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LobbyController : NetworkBehaviour
{
    public static LobbyController Instance;
    public bool gameStarted = false;
    private void Awake() => Instance = this;
    private void Start()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        int count = NetworkManager.Singleton.ConnectedClients.Count;
        Debug.Log("Players connected: " + count);

        if (gameStarted || count > MatchmakingManager.Instance.MAX_PLAYERS)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
        else if (count == MatchmakingManager.Instance.MAX_PLAYERS)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (IsServer)
        {
            MatchManager.Instance.BeginMatch();
            StartGameClientRpc();
            gameStarted = true;
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        StartGame();
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        // Tell everyone that the match is starting
        var ui = FindFirstObjectByType<WaitingUI>();
        if (ui != null)
            ui.HideWaitingScreen();
    }
}
