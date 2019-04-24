using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //TODO: Current Score
    //TODO: High Score
    //TODO: List of all scores
    //TODO: Static method to call Game a Game Over, pass failure point
    //TODO: Display ranking and failure point on Game Over
    //TODO: On Game Over, wait for input to continue (R?)
    // Use this for initialization
    [SerializeField] GameObject Snake;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: Update score based on Snake's length
        //TODO: Give FoodSpawner a function to take in the Snake's length to affect spawn speed
	}

    public static void InitiateGameOver(string failurePoint)
    {

    }
}
