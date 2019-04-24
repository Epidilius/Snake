using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    [SerializeField] private GameObject SnakeFoodPrefab;

    [SerializeField] private Transform BorderTop;
    [SerializeField] private Transform BorderBottom;
    [SerializeField] private Transform BorderLeft;
    [SerializeField] private Transform BorderRight;


    // Use this for initialization
    void Start () {
        //TODO: Move to update, base on how large Snake is
        InvokeRepeating("InstantiateFood", 3, 4);
    }
	
    private void InstantiateFood()
    {
        int x = (int)Random.Range(BorderLeft.position.x, BorderRight.position.x);
        int y = (int)Random.Range(BorderBottom.position.y, BorderTop.position.y);
        
        Instantiate(SnakeFoodPrefab, new Vector2(x, y), Quaternion.identity);
    }
}
