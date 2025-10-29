using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "Quiz/Question Data")]
public class QuestionData : ScriptableObject
{
    [TextArea(2,6)]
    public string question;
    public string[] options = new string[4];
    public int correctIndex = 0;
}
