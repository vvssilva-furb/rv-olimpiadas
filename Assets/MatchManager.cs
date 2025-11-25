using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : NetworkBehaviour
{
    public static MatchManager Instance;

    private int playerProgress = 0;
    private List<ulong> activePlayers = new();

    public Transform[] roomSpawnPoints;
    public Transform sharedRoomSpawn;

    private void Awake() => Instance = this;

    public void BeginMatch(List<ulong> players)
    {
        activePlayers = players;

        playerProgress = 0;

        foreach (var clientId in players)
        {

            AssignPlayerToRoom(clientId);
        }
    }

    private void AssignPlayerToRoom(ulong clientId)
    {
        var playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        int index = activePlayers.IndexOf(clientId);

        var spawn = roomSpawnPoints[index];
        var pc = playerObject.GetComponent<PlayerController>();

        pc.TeleportClientRpc(spawn.position);
    }

    [ServerRpc(RequireOwnership = false)]
    public void NotifyQuestionProgressServerRpc()
    {
        if (!IsServer) return;

        playerProgress++;

        Debug.Log($"Player progress updated: {playerProgress}/{activePlayers.Count}");
        if (AllPlayersFinished())
        {
            MoveToSharedRoom();
        }
    }

    private bool AllPlayersFinished()
    {
        return playerProgress == activePlayers.Count;
    }

    private void MoveToSharedRoom()
    {
        foreach (var clientId in activePlayers)
        {
            var player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            var pc = player.GetComponent<PlayerController>();

            pc.TeleportClientRpc(sharedRoomSpawn.position);
        }

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
        yield return new WaitForSeconds(30f);
        DisconnectAllPlayers();
    }

    private void DisconnectAllPlayers()
    {
        foreach (var clientId in activePlayers)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
        }
    }
}
