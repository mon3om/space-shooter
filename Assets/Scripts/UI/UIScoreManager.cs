using UnityEngine;
using TMPro;

public class UIScoreManager : MonoBehaviour
{
    private static TMP_Text scoreText;
    private static int score = 0;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    public static void UpdateScore(int score)
    {
        UIScoreManager.score += score;
        scoreText.text = "Score: " + UIScoreManager.score.ToString();
    }

    public static void ResetScore()
    {
        score = 0;
    }
}