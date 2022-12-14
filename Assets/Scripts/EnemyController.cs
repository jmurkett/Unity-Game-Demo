using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private List<Enemy> enemies;

    [SerializeField] private float baseTimeUntilNextSpawn;
    [SerializeField] private float minimumTimeUntilNextSpawn;
    private float timeUntilNextSpawn;

    private GameController gameController;

    private void Awake()
    {
        enemies = new List<Enemy>();
        timeUntilNextSpawn = baseTimeUntilNextSpawn;
    }

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        SpawnEnemy();
    }

    private void SpawnEnemy() 
    {
        Collider2D[] collider2Ds = new Collider2D[1];
        Vector2 spawnPosition;

        // Choose a random position and ensure that it is not colliding with any other objects
        do
        {
            float xPosition = Random.Range(-7.41f, 7.41f);
            float yPosition = Random.Range(-3.2f, 3.2f);
            spawnPosition = new Vector2(xPosition, yPosition);

        } while (Physics2D.OverlapCircle(spawnPosition, 1, new ContactFilter2D().NoFilter(), collider2Ds) != 0);

        Enemy enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform).GetComponent<Enemy>();

        enemies.Add(enemy);
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SpawnEnemy();
        }

        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn < 0)
        {
            SpawnEnemy();
            // The time until next spawn decreases as the player's score increases
            // But the time cannot be less than the minimum time until next spawn
            timeUntilNextSpawn = Mathf.Max(baseTimeUntilNextSpawn - gameController.GetScore() / 10, minimumTimeUntilNextSpawn);
        }
    }

    public void IterateEnemyStates()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.IterateState();
        } 
    }

    public void DestroyEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

}
