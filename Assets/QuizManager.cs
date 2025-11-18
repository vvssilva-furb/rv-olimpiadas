using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons = new Button[4];
public TextMeshProUGUI popup;
    public float displayDuration = 2.5f;
    public AneisOlimpicosController aneisOlimpicosController;
    public AneisOlimpicosController.Ring ringIndex;

    private QuestionData currentQuestion;
    private int correctAnswersCount = 0;



    void Start()
    {
popup.gameObject.SetActive(false);
    }

    private Coroutine currentRoutine;

    public void EraseQuestion()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(EraseQuestionRoutine());
    }

    private IEnumerator EraseQuestionRoutine()
    {
        yield return new WaitForSeconds(displayDuration);

        questionText.text = "";
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
            currentQuestion.answered = true;
            currentQuestion = null;
            correctAnswersCount++;

            FindFirstObjectByType<PopupTextController>().ShowMessage(correct ? "CERTO" : "ERRADO", correct ? Color.green : Color.red, displayDuration);
            EraseQuestion();

            if (correctAnswersCount == 3)
            {
                aneisOlimpicosController.ColorRing(ringIndex);
            }
        }
        else
        {
            FindFirstObjectByType<PopupTextController>().ShowMessage(correct ? "CERTO" : "ERRADO", correct ? Color.green : Color.red, displayDuration);
        }
    }

    // Optional helper to close popup from a button
    public void ClosePopup()
    {
        popup.gameObject.SetActive(false);
    }
}
