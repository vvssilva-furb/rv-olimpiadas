using Unity.Netcode;
using UnityEngine;

public class WaitingUI : MonoBehaviour
{
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private GameObject startNowButton;

    private void Start()
    {
        waitingPanel.SetActive(true);
    }

    public void HideWaitingScreen()
    {
        waitingPanel.SetActive(false);
    }
    public void StartNowButtonPressed()
    {
        LobbyController.Instance.StartGameServerRpc();
    }
}
