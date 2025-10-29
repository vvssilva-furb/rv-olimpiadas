using UnityEngine;

public class CursorRaycaster : MonoBehaviour
{
    public LayerMask hoverLayer; // assign layers that include your squares

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, hoverLayer))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red); // visualize in Scene view
            Debug.Log("Cursor hovering over: " + hit.collider.name);
        }
    }
}
