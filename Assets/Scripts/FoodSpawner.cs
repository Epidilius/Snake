using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    private const double DEFAULT_SPAWN_TIME = 3d;

    [SerializeField] private GameObject SnakeFoodPrefab;

    [SerializeField] private Transform BorderTop;
    [SerializeField] private Transform BorderBottom;
    [SerializeField] private Transform BorderLeft;
    [SerializeField] private Transform BorderRight;

    private int SnakeLength;

    private double TimeSinceSpawn;

    private List<GameObject> FoodPieces;
    private Stack<GameObject> UnusedFoodPieces;

    private bool ShouldSpawnFood;

    // Use this for initialization
    void Start () {
        FoodPieces = new List<GameObject>();
        UnusedFoodPieces = new Stack<GameObject>();

        ResetFoodSpawner();

        StopSpawningFood();
    }

    private void Update()
    {
        TimeSinceSpawn += Time.deltaTime;
        if (TimeSinceSpawn > (DEFAULT_SPAWN_TIME / SnakeLength))
        {
            SpawnFood();
            TimeSinceSpawn = 0d;
        }
    }
    
    public void ResetFoodSpawner()
    {
        TimeSinceSpawn = 0d;

        SnakeLength = 0;

        RemoveFood();

        StartSpawningFood();
    }
    public void StartSpawningFood()
    {
        ShouldSpawnFood = true;
    }
    public void StopSpawningFood()
    {
        ShouldSpawnFood = false;
    }

    public void SetSnakeLength(int tailLength)
    {
        SnakeLength = tailLength;
    }

    private void SpawnFood()
    {
        if (ShouldSpawnFood == false) return;

        GameObject foodPiece;
        int x = (int)Random.Range(BorderLeft.position.x, BorderRight.position.x);
        int y = (int)Random.Range(BorderBottom.position.y, BorderTop.position.y);

        if (UnusedFoodPieces.Count > 0) foodPiece = UnusedFoodPieces.Pop();
        else foodPiece = Instantiate(SnakeFoodPrefab);

        foodPiece.SetActive(true);

        foodPiece.transform.position = new Vector2(x, y);
        foodPiece.transform.rotation = Quaternion.identity;

        FoodPieces.Add(foodPiece);
    }
    private void RemoveFood()
    {
        for (int i = 0; i < FoodPieces.Count; i++)
        {
            GameObject foodPiece = FoodPieces[i];
            foodPiece.SetActive(false);
            UnusedFoodPieces.Push(foodPiece);
        }

        FoodPieces.Clear();
    }
}
