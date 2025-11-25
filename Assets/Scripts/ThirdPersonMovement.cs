using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : NetworkBehaviour
{
    public InputActionReference move; // assign in prefab
    public Transform cameraTarget;    // should be a child camera root (local) in the prefab
    public float speed = 5f;
    public float rotationSpeed = 720f;
    public float gravity = -9.81f;
    public bool movementLocked;

    CharacterController controller;
    float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // IMPORTANT: Clone the action before enabling it
            if (move != null)
            {
                move = ScriptableObject.Instantiate(move);
                move.action.Enable();
            }

            if (cameraTarget == null)
            {
                Camera cam = GetComponentInChildren<Camera>(true);
                if (cam != null) cameraTarget = cam.transform;
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if (move != null) move.action.Disable();
    }

    void Update()
    {
        // Only the local player should read input and perform local movement
        if (!IsOwner) return;
        if (movementLocked) return;
        if (!controller.enabled) return;

        Vector2 input = Vector2.zero;
        if (move != null) input = move.action.ReadValue<Vector2>();

        // Build camera-relative move direction
        Vector3 camForward = cameraTarget.forward;
        camForward.y = 0;
        Vector3 camRight = cameraTarget.right;
        camRight.y = 0;

        Vector3 moveDir =
            camForward.normalized * input.y +
            camRight.normalized * input.x;

        // Only rotate if the player is intentionally turning (horizontal axis)
        if (Mathf.Abs(input.x) > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        if (controller.isGrounded) verticalVelocity = -0.5f;
        else verticalVelocity += gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;
        controller.Move(moveDir * speed * Time.deltaTime);
    }
}
