using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : NetworkBehaviour
{
    public InputActionReference move;
    public Transform cameraTarget;
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

    void Update()
    {
        // Only the local player moves itself
        if (!IsOwner)
            return;

        if (movementLocked)
            return;

        if (!controller.enabled)
            return;

        Vector2 input = move.action.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Vector3 camForward = cameraTarget.forward;
            camForward.y = 0;
            Quaternion camRot = Quaternion.LookRotation(camForward);
            moveDir = camRot * moveDir;

            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        if (controller.isGrounded)
            verticalVelocity = -0.5f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;

        controller.Move(moveDir * speed * Time.deltaTime);
    }
}
