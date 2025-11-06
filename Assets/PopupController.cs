using UnityEngine;
using TMPro;
using System.Collections;

public class PopupTextController : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public float displayDuration = 4f;

    private Coroutine currentRoutine;

    public void ShowMessage(string message, Color color)
    {
        // If another message is already showing, stop it
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowMessageRoutine(message, color));
    }

    private IEnumerator ShowMessageRoutine(string message, Color color)
    {
        popupText.text = message;
        popupText.color = color;
        popupText.enabled = true;

        yield return new WaitForSeconds(displayDuration);

        popupText.enabled = false;
        currentRoutine = null;
    }
}
