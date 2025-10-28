using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonLook : MonoBehaviour
{
    [Header("Input System")]
    public InputAction look;  // define or bind this in inspector
    [Header("Settings")]
    public float sensitivity = 150f;
    public float minPitch = -60f;
    public float maxPitch = 75f;

    float yaw;
    float pitch;

    void OnEnable() => look.Enable();
    void OnDisable() => look.Disable();

    void Update()
    {
        Vector2 delta = look.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
        yaw += delta.x;
        pitch -= delta.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
