using System.Collections;
using UnityEngine;

public class TeleportOnStart : MonoBehaviour
{
    public Transform spawnPoint;

    private IEnumerator Start()
    {
        // Wait one frame so the controller initializes
        yield return null;
        
        GameObject player = GameObject.Find("Player");

        player.transform.position = spawnPoint.position;

        // Optional: also zero velocity BEFORE the controller moves again
        var controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.transform.position = spawnPoint.position;
        controller.enabled = true;
    }
}
