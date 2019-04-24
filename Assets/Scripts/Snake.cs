using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour {

    private Vector2 MovementDirection;
    private Vector2 OldHeadPosition;

    private List<GameObject> TailPieces;

    private bool ShouldAddTail;

    [SerializeField] GameObject SnakeTailPrefab;

	// Use this for initialization
	void Start () {
        MovementDirection = Vector2.right;

        TailPieces = new List<GameObject>();
        OldHeadPosition = transform.position;

        ShouldAddTail = false;
        //TODO: Move to Update
        InvokeRepeating("MoveSnake", 0.1f, 0.1f);
    }
	
	// Update is called once per frame
	void Update () {
        //TODO: Arrow keys
        if      (Input.GetKey(KeyCode.W)) MovementDirection = Vector2.up;
        else if (Input.GetKey(KeyCode.S)) MovementDirection = Vector2.down;
        else if (Input.GetKey(KeyCode.A)) MovementDirection = Vector2.left;
        else if (Input.GetKey(KeyCode.D)) MovementDirection = Vector2.right;

        if (ShouldAddTail) AddTailToSnake();

        //Call this on a timeframe based on the amount of tail pieces it has
        //MoveSnake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.ToLower() == "food")
        {
            ShouldAddTail = true;//TODO: Just call the func here?
            Destroy(collision.gameObject);
        }
        else
        {
            //game over
        }
    }

    private void MoveSnake()
    {
        MoveHead();
        MoveTail();
    }
    private void MoveHead()
    {
        OldHeadPosition = transform.position;
        transform.Translate(MovementDirection);
    }
    private void MoveTail()
    {
        if (TailPieces.Count == 0) return;

        int lastIndex = TailPieces.Count - 1;

        TailPieces[lastIndex].transform.position = OldHeadPosition;
        
        TailPieces.Insert(0, TailPieces[lastIndex]);
        TailPieces.RemoveAt(TailPieces.Count - 1);
    }

    private void AddTailToSnake()
    {
        GameObject tail = Instantiate(SnakeTailPrefab, OldHeadPosition, Quaternion.identity);
        TailPieces.Insert(0, tail);
        ShouldAddTail = false;
    }

    public int GetSize()
    {
        return TailPieces.Count + 1;
    }
}
