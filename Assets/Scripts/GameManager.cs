using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SnakeController snakeController;
    public FoodSpawner foodSpawner;

    public TMP_Text scoreText;
    public TMP_Text levelText;

    private int score = 0;
    private int level = 1;
    private int pointsPerFood = 10;
    private int levelUpThreshold = 50; // Очки для повышения уровня

    void Start()
    {
        UpdateUI();
        foodSpawner.SpawnFood();
    }

    void Update()
    {
        if (snakeController.GetSnakeLenght() > 0)
        {
            var headPos = snakeController.GetHeadPosition();
            var foodPos = foodSpawner.GetCurrentFoodTransform().position;
            
            float epsilon = 0.01f;
            
            if (Mathf.Abs(headPos.x - foodPos.x) < epsilon && Mathf.Abs(headPos.y - foodPos.y) < epsilon)
            {
                Debug.Log("Еда съедена!");
                snakeController.Grow();
                foodSpawner.SpawnFood();

                // Обновляем очки
                score += pointsPerFood;
                CheckLevelUp();
                UpdateUI();
            }
        }
    }

    void CheckLevelUp()
    {
        if (score >= level * levelUpThreshold)
        {
            level++;
            // Можно увеличить скорость или добавить другие изменения
            snakeController.moveSpeed = Mathf.Max(0.05f, snakeController.moveSpeed - 0.02f);
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Очки: " + score;
        if (levelText != null)
            levelText.text = "Уровень: " + level;
    }
}