using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    private float defaultTimeScale;
    private float defaultFixedDeltaTime;
    [SerializeField] private float slowMotionFactor;

    private void Awake()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.SetText("Score: " + score.ToString());
    }

    public void StartSlowMotion()
    {
        Time.timeScale = slowMotionFactor * defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * slowMotionFactor;
    }

    public void StopSlowMotion()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }

    public int GetScore() { return score; }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}
