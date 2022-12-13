using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Vulnerable, // Enemy will be destroyed if collided with
        Invulnerable // Player will be destroyed if collided with
    }

    private EnemyState enemyState;

    private SpriteRenderer spriteRenderer;
    private EnemyController enemyController;
    private GameController gameController;
    private CircleCollider2D circleCollider2D;

    private bool enemyActivated = false;
    [SerializeField] private float timeUntilActivated;

    // The intial speed of the enemy when it is activated
    [SerializeField] private float startingSpeed;

    private void Awake()
    {
        // Initial enemy state is vulernable
        enemyState = EnemyState.Vulnerable;
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
            // Check if the enemy has become activate yet
            if (timeUntilActivated < 0)
            {
                enemyActivated = true;
                circleCollider2D.enabled = true;
                SetSpriteColor();

                // Give the enemy an initial force
                Vector2 enemyStartingForce = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                enemyStartingForce.Normalize();
                gameObject.GetComponent<Rigidbody2D>().AddForce(startingSpeed * enemyStartingForce);
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
        if (collision.gameObject.CompareTag("Player") && enemyState == EnemyState.Vulnerable)
        {
            gameController.IncreaseScore(2);
            enemyController.IterateEnemyStates();
            enemyController.DestroyEnemy(this);
        }
        else if (collision.gameObject.CompareTag("Player") && enemyState == EnemyState.Invulnerable)
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
        // If the enemy is not activated yet then reduce the opacity
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
