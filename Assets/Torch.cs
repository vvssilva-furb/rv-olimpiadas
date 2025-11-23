using UnityEngine;
using UnityEngine.SceneManagement;

public class Torch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnMouseEnter()
    {
        FindFirstObjectByType<PopupTextController>().ShowMessage("Deseja sair?", Color.white, float.PositiveInfinity);
    }

    void OnMouseExit()
    {
        FindFirstObjectByType<PopupTextController>().EraseMessage();
    }

    void OnMouseDown()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
