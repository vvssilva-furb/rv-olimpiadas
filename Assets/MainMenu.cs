using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Demo"; // the name of your main game scene

    public void StartGame()
    {
        NetworkManager.Singleton.StartClient();
        
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}. Returning to main menu.");
        SceneManager.LoadScene("MainMenu"); // your main menu scene name
    }
}
