using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Tutorials.Core.Editor;
using UnityEditor.PackageManager;
using UnityEngine;
using static AneisOlimpicosController;

public class MatchManager : NetworkBehaviour
{
    public static MatchManager Instance;

    private int playerProgress = 0;
    private List<ulong> activePlayers = new();

    public Transform[] roomSpawnPoints;
    public Transform sharedRoomSpawn;

    private void Awake() => Instance = this;

    public void BeginMatch()
    {
        playerProgress = 0;

        var clients = NetworkManager.Singleton.ConnectedClientsList;

        for (int i = 0; i < clients.Count; i++)
        {
            var client = clients[i];
            var playerObject = client.PlayerObject;

            if (playerObject == null)
            {
                Debug.LogWarning($"Client {client.ClientId} has no PlayerObject!");
                continue;
            }

            // Get spawn point based on index
            var spawn = roomSpawnPoints[i];

            // Teleport
            var pc = playerObject.GetComponent<PlayerController>();
            pc.TeleportClientRpc(spawn.position);
            pc.OnMatchStartedClientRpc();
        }

        int remainingRings = 5 - clients.Count;

        Ring[] allRings = new Ring[]
        {
            Ring.Americas,
            Ring.Oceania,
            Ring.Africa,
            Ring.Asia,
            Ring.Europa,
        };

        for (int i = 0; i < remainingRings; i++)
        {
            AneisOlimpicosController.Instance.ColorRingClientRpc(allRings[i]);
        }
    }

    [ClientRpc]
    public void ShowMessageClientRpc(string message, ClientRpcParams rpcParams = default)
    {
        if (message.IsNotNullOrEmpty())
        {
            FindFirstObjectByType<PopupTextController>().ShowMessage(message, Color.white, float.PositiveInfinity);
        }
        else
        {
            FindFirstObjectByType<PopupTextController>().EraseMessage();

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void NotifyQuestionProgressServerRpc(ServerRpcParams serverParams = default)
    {
        if (!IsServer) return;

        playerProgress++;
        
        Debug.Log($"Player progress updated: {playerProgress}/{NetworkManager.Singleton.ConnectedClientsList.Count}");

        var clients = NetworkManager.Singleton.ConnectedClientsList;
        int index = -1;
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].ClientId == serverParams.Receive.SenderClientId)
            {
                index = i;
                break;
            }
        }
        
        AneisOlimpicosController.Instance.ColorRingClientRpc((Ring)index);

        if (AllPlayersFinished())
        {
            ShowMessageClientRpc("");
            MoveToSharedRoom();
        }
        else
        {

            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { serverParams.Receive.SenderClientId }
                }
            };

            // Call RPC — only the specified client will receive it
            GetComponent<MatchManager>().ShowMessageClientRpc("Parabéns, você completou a sua sala! Esperando os outros jogadores terminarem...", clientRpcParams);
        }
    }

    private bool AllPlayersFinished()
    {
        return playerProgress == NetworkManager.Singleton.ConnectedClientsList.Count;
    }

    private void MoveToSharedRoom()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var player = client.PlayerObject;
            var pc = player.GetComponent<PlayerController>();

            pc.TeleportClientRpc(sharedRoomSpawn.position);
        }

        if (NetworkManager.Singleton.ConnectedClientsList.Count > 1)
            ShowMessageClientRpc("Parabéns, vocês venceram! Voltando ao menu principal em breve...");
        else
            ShowMessageClientRpc("Parabéns, você venceu! Voltando ao menu principal em breve...");


            StartCoroutine(PostGameCountdown());
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            Debug.Log("Spawned matchmanager");
            DontDestroyOnLoad(gameObject);
        }
    }

    private IEnumerator PostGameCountdown()
    {
        yield return new WaitForSeconds(10f);
        DisconnectAllPlayers();

        LobbyController.Instance.gameStarted = false;
    }

    private void DisconnectAllPlayers()
    {
        // Make a copy so disconnecting doesn't modify the list you're iterating
        var clients = new List<NetworkClient>(NetworkManager.Singleton.ConnectedClientsList);

        foreach (var client in clients)
        {
            NetworkManager.Singleton.DisconnectClient(client.ClientId);
        }
    }
}
