using UnityEngine;

public class CursorLock : MonoBehaviour
{
    void Start()
    {
        Debug.Log("CursorLock script started.");
        // Lock cursor to the center and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Optionally unlock when pressing Escape (for menus)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
