using UnityEngine;
using TMPro;
using System.Collections;

public class PopupTextController : MonoBehaviour
{
    public TextMeshProUGUI popupText;

    private Coroutine currentRoutine;

    public void ShowMessage(string message, Color color, float displayDuration)
    {
        popupText.text = message;
        popupText.color = color;
        popupText.enabled = true;
    }

    public void EraseMessage()
    {
        popupText.enabled = false;
    }
}
