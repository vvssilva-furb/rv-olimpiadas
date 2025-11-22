using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public QuizManager quizManager;

    private Renderer rend;
    private Color originalColor;
    public Color highlightColor = Color.red;
	public int idx;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        quizManager.OnAnswerMouseEnter(idx);
        rend.material.color = highlightColor;

    }

    void OnMouseExit()
    {
        quizManager.OnAnswerMouseExit(idx);
        rend.material.color = originalColor;
    }

    // Ensure the object has a Collider for OnMouseDown to work
    private void OnMouseDown()
    {

            quizManager.OnAnswerClicked(idx);

    }
}
