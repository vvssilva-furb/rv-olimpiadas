using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class ThirdPersonLook : NetworkBehaviour
{
    [Header("Input System")]
    public InputActionReference look; // assign an InputAction asset reference in prefab

    [Header("Settings")]
    public float sensitivity = 150f;
    public float minPitch = -60f;
    public float maxPitch = 75f;

    float yaw;
    float pitch;

    Camera ownedCamera;

    public override void OnNetworkSpawn()
    {
        // Cache camera (should be a child camera in the player prefab)
        ownedCamera = GetComponentInChildren<Camera>(true);

        if (IsOwner)
        {
            // Enable camera so only local player renders this camera
            if (ownedCamera) ownedCamera.enabled = true;

            // Enable the input action for this instance
            if (look != null) look.action.Enable();
        }
    }

    public override void OnNetworkDespawn()
    {
        // Clean up
        if (look != null) look.action.Disable();
    }

    void Update()
    {
        // Prevent non-owners from reading input / rotating this transform
        if (!IsOwner) return;

        if (look == null) return;
        Vector2 delta = look.action.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
        yaw += delta.x;
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
