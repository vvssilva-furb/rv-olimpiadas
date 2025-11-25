using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine;

public class MatchmakingManager : NetworkBehaviour
{
    public static MatchmakingManager Instance;

    private List<ulong> queue = new();

    private void Awake() => Instance = this;

    public void AddPlayerToQueue(ulong clientId)
    {
        if (!IsServer) return;

        if (!queue.Contains(clientId))
            queue.Add(clientId);

        TryStartMatch();
    }

    private void TryStartMatch()
    {
        if (queue.Count < 1) return;

        // Get first 2
        var players = new List<ulong>() { queue[0] };
        queue.RemoveRange(0, 1);

        StartMatch(players);
    }

    private void StartMatch(List<ulong> players)
    {
        MatchManager.Instance.BeginMatch(players);
    }
}
