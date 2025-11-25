using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayBootstrap : MonoBehaviour
{
    void Start()
    {
        string path = Path.Combine(Application.dataPath, "../is_server");

        if (File.Exists(path))
        {
            Debug.Log("Starting SERVER");
            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.SceneManager.LoadScene("Demo", LoadSceneMode.Single);
        }
        //}
        //else
        //{
        //    Debug.Log("Starting CLIENT");
        //    NetworkManager.Singleton.StartClient();
        //}
    }
}
