using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons = new Button[4];
public TextMeshProUGUI popup;


    private QuestionData currentQuestion;



void Start()
    {
popup.gameObject.SetActive(false);
        // Safe to access other components, scene objects, etc.
        Debug.Log("Start: MyComponent fully initialized");
    }

    public void ShowQuestion(QuestionData question)
    {
        if (question == null) return;

        currentQuestion = question;

	string _questionText = question.question + "\n";
        for (int i = 0; i < question.options.Length; i++)
        {
            _questionText += question.options[i]+  "\n";
        }

	questionText.text = _questionText;

        popup.gameObject.SetActive(false);
    }

    public void OnAnswerClicked(int index)
    {
Debug.Log("answer clicked");
        if (currentQuestion == null) return;

	popup.gameObject.SetActive(true);

        bool correct = (index == currentQuestion.correctIndex);
        if (correct)
        {
            
		popup.text = "CERTO";
        }
        else
        {
            popup.text = "ERRADO";
        }
    }

    // Optional helper to close popup from a button
    public void ClosePopup()
    {
        popup.gameObject.SetActive(false);
    }
}
