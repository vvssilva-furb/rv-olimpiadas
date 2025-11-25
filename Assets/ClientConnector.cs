using Unity.Netcode;
using UnityEngine;

public class ClientConnector : NetworkBehaviour
{
    public static ClientConnector Instance;

    private void Awake() => Instance = this;

    [ServerRpc(RequireOwnership = false)]
    public void RequestJoinQueueServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        MatchmakingManager.Instance.AddPlayerToQueue(clientId);
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {

            Debug.Log("Spawned clientconnector");
            DontDestroyOnLoad(gameObject);
        }
    }

    [ClientRpc]
    public void StartMatchClientRpc()
    {
        //// Hide waiting UI
        //var ui = FindObjectOfType<MainMenuUI>();
        //if (ui != null)
        //    ui.HideWaitingUI();
    }
}
