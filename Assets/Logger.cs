using Unity.Netcode;
using UnityEngine;

public class NetworkDebugLogger : MonoBehaviour
{
    void Start()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("[LOGGER] No NetworkManager found in scene!");
            return;
        }

        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

        Debug.Log("[LOGGER] NetworkDebugLogger initialized.");
    }

    private void OnServerStarted()
    {
        Debug.Log("[SERVER] Server started");
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"[SERVER] Client connected: {clientId}");

        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
        {
            if (client.PlayerObject != null)
            {
                Debug.Log($"[SERVER] Spawned player for client {clientId}");
            }
            else
            {
                Debug.Log($"[SERVER] No PlayerObject for client {clientId}");
            }
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"[SERVER] Client disconnected: {clientId}");
    }
}
