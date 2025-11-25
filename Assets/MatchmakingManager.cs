using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingManager : NetworkBehaviour
{
    public int MAX_PLAYERS = 5;
    public static MatchmakingManager Instance;

    private List<ulong> queue = new();

    private void Awake()  {
        Instance = this;
        }

    public void AddPlayerToQueue(ulong clientId)
    {
        if (!IsServer) return;

        if (!queue.Contains(clientId))
            queue.Add(clientId);

        TryStartMatch();
    }

    private void TryStartMatch()
    {
        StartMatch(queue);
        queue.Clear();
    }

    private void StartMatch(List<ulong> players)
    {
        MatchManager.Instance.BeginMatch();
    }
}
