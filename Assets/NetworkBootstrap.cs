using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    public bool isServerBuild; // toggle in inspector

    void Start()
    {
#if UNITY_SERVER
    NetworkManager.Singleton.StartServer();
#else
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData("127.0.0.1", 7777);

        // In Multiplay Play Mode, Unity auto-starts server in editor,
        // and auto-connects clients when StartClient() is used.

        NetworkManager.Singleton.StartClient();
#endif
    }
}
