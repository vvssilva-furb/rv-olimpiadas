using UnityEngine;

public class TeleportOnStart : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        Debug.Log("First teleport");
        transform.position = spawnPoint.position;
    }
}
