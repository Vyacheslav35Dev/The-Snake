using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public Vector2Int gridSize = new Vector2Int(20, 20);

    private GameObject currentFood;

    public Transform GetCurrentFoodTransform()
    {
        return currentFood.transform;
    }

    public void SpawnFood()
    {
        if (currentFood != null)
            Destroy(currentFood);

        int x = Random.Range(0, gridSize.x);
        int y = Random.Range(0, gridSize.y);

        currentFood = Instantiate(foodPrefab, new Vector3(x, y, 0), Quaternion.identity);
    }
}