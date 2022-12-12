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

    private void Awake()
    {
        enemyState = EnemyState.Vulnerable;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSpriteColor();
    }

    private void Start()
    {
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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
        if (collision.gameObject.tag == "Player" && enemyState== EnemyState.Vulnerable)
        {
            gameController.IncreaseScore(1);
            enemyController.DestroyEnemy(this);
        }
    }

    public EnemyState GetEnemyState()
    {
        return enemyState;
    }

    private void SetSpriteColor()
    {
        switch (enemyState)
        {
            case EnemyState.Vulnerable:
                spriteRenderer.color = Color.blue;
                break;
            case EnemyState.Invulnerable:
                spriteRenderer.color = Color.red;
                break;
        }
    }
}
