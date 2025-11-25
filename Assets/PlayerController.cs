using System.Collections;
using Unity.Cinemachine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public ThirdPersonMovement movement;
    public ThirdPersonLook look;
    public CinemachineCamera vcam;

    public override void OnNetworkSpawn()
    {
        Debug.Log("Spawning player");
        //if (IsServer)
        //{
        //    // server decides spawn position
        //    transform.position = SpawnManager.Instance.GetNextSpawnPosition();
        //    transform.rotation = SpawnManager.Instance.GetNextSpawnRotation();
        //}

        //if (IsOwner)
        //{
        //    TeleportClientRpc(transform.position);
        //}

        if (IsOwner)
        {
            vcam.gameObject.SetActive(true);
            vcam.Follow = look.transform;
            vcam.LookAt = look.transform;
        }
        else
        {
            vcam.gameObject.SetActive(false);
        }

        movement.movementLocked = true;

        // LOCAL PLAYER SETUP
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //ClientConnector.Instance.RequestJoinQueueServerRpc();
    }

    [ClientRpc]
    public void OnMatchStartedClientRpc()
    {
        movement.movementLocked = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    [ClientRpc]
    public void TeleportClientRpc(Vector3 pos)
    {
        StartCoroutine(TeleportRoutine(pos));
    }

    private IEnumerator TeleportRoutine(Vector3 pos)
    {
        Debug.Log("teleporting player");
        var nt = GetComponent<NetworkTransform>();
        var cc = GetComponent<CharacterController>();

        nt.enabled = false;
        cc.enabled = false;
        cc.detectCollisions = false;
        movement.movementLocked = true;

        transform.position = pos;

        yield return new WaitForEndOfFrame();
        
        movement.movementLocked = false;
        cc.detectCollisions = true;
        cc.enabled = true;
        nt.enabled = true;
    }
}