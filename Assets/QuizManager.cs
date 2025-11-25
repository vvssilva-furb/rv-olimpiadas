using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.Netcode;

public class QuizManager : NetworkBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] optionButtons = new Button[4];
    public float displayDuration = 2.5f;
    public AneisOlimpicosController aneisOlimpicosController;
    public AneisOlimpicosController.Ring ringIndex;
    public GameObject player;
    public Transform next;

    private QuestionData currentQuestion;
    QuestionSquare currentSquare;
    private int correctAnswersCount = 0;



    void Start()
    {
    }

    private Coroutine currentMessageRoutine;
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

    private IEnumerator MoveToNext()
    {
        yield return new WaitForSeconds(displayDuration);

        MatchManager.Instance.NotifyQuestionProgressServerRpc();

        //CharacterController cc = player.GetComponent<CharacterController>();
        //cc.enabled = false;               // ❤️ stop snap-back

        //player.transform.position = next.position;

        //yield return null;                // wait one frame
        //cc.enabled = true;                // ❤️ re-enable safely
    }

    public void ShowQuestion(QuestionSquare square, QuestionData question)
    {
        if (question == null) return;

        currentSquare = square;
        currentQuestion = question;
        questionText.text = question.question;

    }

    private IEnumerator ShowMessageRoutine(string message, Color color, float displayDuration)
    {
        FindFirstObjectByType<PopupTextController>().ShowMessage(message, color, displayDuration);

        yield return new WaitForSeconds(displayDuration);

        FindFirstObjectByType<PopupTextController>().EraseMessage();
        currentMessageRoutine = null;
    }

    public void ShowMessage(string message, Color color, float displayDuration)
    {
        // If another message is already showing, stop it
        if (currentMessageRoutine != null)
            StopCoroutine(currentMessageRoutine);

        currentMessageRoutine = StartCoroutine(ShowMessageRoutine(message, color, displayDuration));
    }

    public void OnAnswerMouseEnter(int index)
    {
        if (currentQuestion == null || currentMessageRoutine != null) return;

        FindFirstObjectByType<PopupTextController>().ShowMessage(currentQuestion.options[index], Color.white, float.PositiveInfinity);
    }

    public void OnAnswerMouseExit(int index)
    {
        if (currentQuestion == null || currentMessageRoutine != null) return;

        FindFirstObjectByType<PopupTextController>().EraseMessage();
    }

    public bool OnAnswerClicked(int index)
    {
        if (currentQuestion == null) return false;

        bool correct = (index == currentQuestion.correctIndex);

        if (correct)
        {
            currentQuestion.answered = true;
            currentQuestion = null;
            correctAnswersCount++;

            ShowMessage(correct ? "CERTO" : "ERRADO", correct ? Color.green : Color.red, displayDuration);
            EraseQuestion();
            currentSquare.GetComponent<Renderer>().material.color = new Color(0.133f, 0.294f, 0.196f);

            if (correctAnswersCount == 3)
            {
                StartCoroutine(MoveToNext());
            }

            return true;
        }
        else
        {
            ShowMessage(correct ? "CERTO" : "ERRADO", correct ? Color.green : Color.red, displayDuration);
        }

        return false;
    }

    // Optional helper to close popup from a button
    public void ClosePopup()
    {
    }
}
