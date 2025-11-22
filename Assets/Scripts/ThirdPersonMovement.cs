using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    public InputActionReference move;     // from your Input Action asset
    public Transform cameraTarget;        // assign your CameraTarget
    public float speed = 5f;
    public float rotationSpeed = 720f;    // degrees per second
    public float gravity = -9.81f;

    CharacterController controller;
    float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()  => move.action.Enable();
    void OnDisable() => move.action.Disable();

    void Update()
    {
        if (!controller.enabled)
            return;

        // read movement input (WASD, stick, etc.)
        Vector2 input = move.action.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);

        // make movement relative to the camera
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Vector3 camForward = cameraTarget.forward;
            camForward.y = 0;
            Quaternion camRot = Quaternion.LookRotation(camForward);
            moveDir = camRot * moveDir;

            // rotate the player toward the movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // simple gravity
        if (controller.isGrounded)
            verticalVelocity = -0.5f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;

        // actually move the character
        controller.Move(moveDir * speed * Time.deltaTime);
    }
}
