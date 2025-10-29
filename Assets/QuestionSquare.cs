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
    }

    void OnMouseEnter()
    {
Debug.Log("Hovering: " + name);
        rend.material.color = highlightColor;
    }

    void OnMouseExit()
    {
Debug.Log("NHovering: " + name);
        rend.material.color = originalColor;
    }

    // Ensure the object has a Collider for OnMouseDown to work
    private void OnMouseDown()
    {
        // You can add checks here for UI blocking, or require left mouse only
        if (quizManager != null && question != null)
            quizManager.ShowQuestion(question);
    }
}
