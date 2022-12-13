using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Vulnerable,
        Invulnerable
    }

    private EnemyState enemyState;
    private SpriteRenderer spriteRenderer;
    private EnemyController enemyController;
    private GameController gameController;

    private CircleCollider2D circleCollider2D;

    private bool enemyActivated = false;
    [SerializeField] private float timeUntilActivated;

    private void Awake()
    {
        enemyState = EnemyState.Invulnerable;
        spriteRenderer = GetComponent<SpriteRenderer>();

        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;

        SetSpriteColor();
    }

    private void Start()
    {
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        if (!enemyActivated)
        {
            timeUntilActivated -= Time.deltaTime;
            if (timeUntilActivated < 0)
            {
                enemyActivated = true;
                circleCollider2D.enabled = true;
                SetSpriteColor();
            }
        }
    }

    public void IterateState()
    {
        if (enemyState == EnemyState.Vulnerable)
        {
            enemyState = EnemyState.Invulnerable;
        }
        else if (enemyState == EnemyState.Invulnerable)
        {
            enemyState= EnemyState.Vulnerable;
        }
        SetSpriteColor();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && enemyState == EnemyState.Vulnerable)
        {
            gameController.IncreaseScore(1);
            enemyController.DestroyEnemy(this);
        }
        else if (collision.gameObject.tag == "Player" && enemyState == EnemyState.Invulnerable)
        {
            gameController.GameOver();
        }
    }

    public EnemyState GetEnemyState()
    {
        return enemyState;
    }

    private void SetSpriteColor()
    {
        float opacity = 1;
        if (!enemyActivated) 
        { 
            opacity = 0.5f;
        }

        Color spriteColor = Color.white;
        switch (enemyState)
        {
            case EnemyState.Vulnerable:
                spriteColor = Color.blue;
                break;
            case EnemyState.Invulnerable:
                spriteColor = Color.red;
                break;
        }
        spriteColor.a = opacity;
        spriteRenderer.color = spriteColor;
    }
}
