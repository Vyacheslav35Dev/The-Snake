using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject segmentPrefab; // Префаб сегмента змейки
    public float moveSpeed = 0.2f;   // Время между движениями
    public Vector2Int gridSize = new Vector2Int(20, 20); // Размер поля

    private List<GameObject> snakeSegments = new List<GameObject>();
    private Vector3 direction = Vector3.right;
    private float timer = 0f;
    private Vector3 currentPosition;

    void Start()
    {
        // Инициализация змейки из одного сегмента
        currentPosition = new Vector3(10, 10);
        GameObject head = Instantiate(segmentPrefab, new Vector3(currentPosition.x, currentPosition.y, 0), Quaternion.identity);
        snakeSegments.Add(head);
    }

    void Update()
    {
        HandleInput();

        timer += Time.deltaTime;
        if (timer >= moveSpeed)
        {
            Move();
            timer = 0f;
        }
    }

    void HandleInput()
    {
        Vector2 inputDir = playerInput.moveInput;

        if (inputDir != Vector2.zero)
        {
            Vector3 newDirection;

            if (Mathf.Abs(inputDir.x) > Mathf.Abs(inputDir.y))
            {
                // Горизонтальное движение
                newDirection = inputDir.x > 0 ? Vector3.right : Vector3.left;
            }
            else
            {
                // Вертикальное движение
                newDirection = inputDir.y > 0 ? Vector3.up : Vector3.down;
            }

            // Проверка, чтобы не двигаться назад
            if (!IsOppositeDirection(newDirection))
            {
                direction = newDirection;
            }
        }
    }

    void Move()
    {
        currentPosition += direction;

        // Проверка выхода за границы и телепортация
        if (currentPosition.x < 0)
        {
            currentPosition.x = gridSize.x - 1;
        }
        else if (currentPosition.x >= gridSize.x)
        {
            currentPosition.x = 0;
        }

        if (currentPosition.y < 0)
        {
            currentPosition.y = gridSize.y - 1;
        }
        else if (currentPosition.y >= gridSize.y)
        {
            currentPosition.y = 0;
        }

        // Проверка столкновения с собой
        foreach (var segment in snakeSegments)
        {
            if ((Vector2)segment.transform.position == new Vector2(currentPosition.x, currentPosition.y))
            {
                Debug.Log("Game Over");
                enabled = false;
                return;
            }
        }

        // Передвижение сегментов
        for (int i = snakeSegments.Count - 1; i > 0; i--)
        {
            snakeSegments[i].transform.position = snakeSegments[i - 1].transform.position;
        }
    
        // Обновляем позицию головы
        snakeSegments[0].transform.position = new Vector3(currentPosition.x, currentPosition.y, 0);
    }

    public Vector3 GetHeadPosition()
    {
        return snakeSegments[0].transform.position;
    }
    
    private bool IsOppositeDirection(Vector3 newDir)
    {
        return (newDir + direction).sqrMagnitude == 0;
    }

    public int GetSnakeLenght()
    {
        return snakeSegments.Count;
    }

    public void Grow()
    {
        // Добавляем новый сегмент в хвост
        var tail = snakeSegments[snakeSegments.Count - 1];
        var newSegment = Instantiate(segmentPrefab, tail.transform.position, Quaternion.identity);
        snakeSegments.Add(newSegment);
    }
}