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

    private bool ShouldSpawnFood;

    // Use this for initialization
    void Start () {
        FoodPieces = new List<GameObject>();

        ResetFoodSpawner();

        StopSpawningFood();
    }

    private void Update()
    {
        TimeSinceSpawn += Time.deltaTime;
        if (TimeSinceSpawn > (DEFAULT_SPAWN_TIME / SnakeLength))
        {
            InstantiateFood();
            TimeSinceSpawn = 0d;
        }
    }

    public void ResetFoodSpawner()
    {
        TimeSinceSpawn = 0d;

        SnakeLength = 0;

        for (int i = 0; i < FoodPieces.Count; i++)
        {
            Destroy(FoodPieces[i]);
        }

        FoodPieces.Clear();

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

    private void InstantiateFood()
    {
        if (ShouldSpawnFood == false) return;

        int x = (int)Random.Range(BorderLeft.position.x, BorderRight.position.x);
        int y = (int)Random.Range(BorderBottom.position.y, BorderTop.position.y);
        
        FoodPieces.Add(Instantiate(SnakeFoodPrefab, new Vector2(x, y), Quaternion.identity));
    }
}
