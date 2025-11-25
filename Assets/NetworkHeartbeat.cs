using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class NetworkHeartbeat : NetworkBehaviour
{
    private float timer;

    [ServerRpc(RequireOwnership = false)]
    void HeartbeatServerRpc() { }

    void Update()
    {
        if (!IsOwner) return;

        timer += Time.deltaTime;
        if (timer >= 10f)        // every 10 seconds
        {
            HeartbeatServerRpc();
            timer = 0;
        }
    }
}
