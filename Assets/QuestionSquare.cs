using UnityEngine;

public class QuestionSquare : MonoBehaviour
{
    public QuestionData question;
    public QuizManager quizManager;

    private Renderer rend;
    private Color originalColor;
    public Color highlightColor = Color.yellow;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        question.answered = false;
    }

    void OnMouseEnter()
    {
if (!question.answered)
{
    rend.material.color = highlightColor;
}
    }

    void OnMouseExit()
    {
        if (!question.answered)
        {
            rend.material.color = originalColor;
        }

    }

    // Ensure the object has a Collider for OnMouseDown to work
    private void OnMouseDown()
    {
if (!question.answered)
{
        // You can add checks here for UI blocking, or require left mouse only
        if (quizManager != null && question != null)
            quizManager.ShowQuestion(this, question);
}
    }
}
