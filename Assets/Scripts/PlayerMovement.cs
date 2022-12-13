using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    private Vector3 mouseStartPosition;
    private Vector3 mouseEndPosition;

    [SerializeField] private GameObject aimingLinePrefab;
    private LineRenderer aimingLineRenderer;

    [SerializeField] private GameObject shootingPowerText;
    private RectTransform shootingPowerTransform;

    [SerializeField] private float maxShootingPower;
    [SerializeField] private float minShootingPower;
    [SerializeField] private float maxLineLength;

    private EnemyController enemyController;
    private GameController gameController;


    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        aimingLineRenderer = Instantiate(aimingLinePrefab, transform.position, Quaternion.identity, transform).GetComponent<LineRenderer>();
        aimingLineRenderer.positionCount = 0;

        // Initially hide the shooting power text
        shootingPowerText.SetActive(false);
        shootingPowerTransform = shootingPowerText.GetComponent<RectTransform>();

        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Update()
    {
        // The player starts aiming
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPosition = GetWorldCoordinate(Input.mousePosition);
            aimingLineRenderer.positionCount = 2;
            enemyController.IterateEnemyStates();
            gameController.StartSlowMotion();

            shootingPowerTransform.position = new Vector3(transform.position.x + 0.6f, transform.position.y, 0);
            shootingPowerText.GetComponent<TextMeshProUGUI>().SetText(GetAimingPower().ToString("0.00"));
            shootingPowerText.SetActive(true);
        }
        // The player stops aiming and fires
        else if (Input.GetMouseButtonUp(0))
        {
            mouseEndPosition = GetWorldCoordinate(Input.mousePosition);
            float shootingPower = Mathf.Lerp(minShootingPower, maxShootingPower, GetAimingPower());

            MovePlayer(mouseStartPosition - mouseEndPosition, shootingPower);
            aimingLineRenderer.positionCount = 0;
            gameController.StopSlowMotion();

            shootingPowerText.SetActive(false);
        }
        // The player continues aiming
        else if (aimingLineRenderer.positionCount == 2)
        {
            Vector2 lineStart = transform.position;
            Vector2 lineEnd = GetWorldCoordinate(Input.mousePosition) - mouseStartPosition + transform.position;
            Vector2 lineDirection = (lineStart - lineEnd);
            Vector2 lineDirectionNormalized = lineDirection.normalized;
            aimingLineRenderer.SetPosition(0, lineStart - lineDirectionNormalized);

            if (lineDirection.magnitude > maxLineLength)
            {
                lineEnd = lineStart - lineDirectionNormalized * maxLineLength;
            }

            aimingLineRenderer.SetPosition(1, lineEnd - lineDirectionNormalized);

            shootingPowerText.GetComponent<TextMeshProUGUI>().SetText(GetAimingPower().ToString("0.00"));
            shootingPowerTransform.position = new Vector3(transform.position.x + 0.6f, transform.position.y, 0);
        }
    }

    private void MovePlayer(Vector2 direction, float power)
    {
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.angularVelocity = 0;
        rigidBody2D.AddForce(power * direction.normalized);
    }

    // Convert mouse position to world position so that game plays the same for different resolutions
    private Vector3 GetWorldCoordinate(Vector3 position)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(worldPosition.x, worldPosition.y, 0);
    }

    private float GetAimingPower()
    {
        Vector2 line = mouseStartPosition - GetWorldCoordinate(Input.mousePosition);
        return Mathf.Min(line.magnitude / maxLineLength, 1);
    }
}
