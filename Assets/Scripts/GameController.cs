using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    // Used for the slowmotion feature while aiming
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;
    [SerializeField] private float slowMotionFactor;

    private void Awake()
    {
        // Get the default time values so we can revert to them when the player is no longer aiming
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

    // Go to the gameover screen
    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}
