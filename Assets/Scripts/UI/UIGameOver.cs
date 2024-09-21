using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highestScoreText;

    private void Awake()
    {
        Instances.UIGameOver = this;
    }

    public void Display()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void SetScoreText(int value, bool newHighScore)
    {
        if (value == 0)
        {
            Debug.Log("Score not displaying cause it's 0");
            return;
        }

        string str = "Score " + UIScoreManager.score + "\n";
        if (newHighScore)
            highestScoreText.DOText(str + "New High Score", 1.3f);
        else
            highestScoreText.DOText(str + "Highest Score " + value, 1.3f);
    }
}
