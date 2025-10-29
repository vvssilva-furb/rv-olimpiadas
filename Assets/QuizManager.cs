using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons = new Button[4];
    public GameObject popup; // a panel that shows when correct, disabled by default

    private QuestionData currentQuestion;

    public void ShowQuestion(QuestionData question)
    {
        if (question == null) return;

        currentQuestion = question;
        questionText.text = question.question;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int idx = i; // local copy for closure
            var btn = optionButtons[i];
            var label = btn.GetComponentInChildren<TextMeshProUGUI>();
            label.text = (i < question.options.Length) ? question.options[i] : "";

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnAnswerClicked(idx));
        }

        popup.SetActive(false);
        gameObject.SetActive(true); // ensure board visible
    }

    private void OnAnswerClicked(int index)
    {
        if (currentQuestion == null) return;

        bool correct = (index == currentQuestion.correctIndex);
        if (correct)
        {
            popup.SetActive(true);
        }
        else
        {
            // optional: give feedback for wrong answer
            // e.g. shake button, show message, etc.
        }
    }

    // Optional helper to close popup from a button
    public void ClosePopup()
    {
        popup.SetActive(false);
    }
}
