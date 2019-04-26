using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour {

    private const double DEFAULT_MOVE_TIME = 0.3d;

    public enum SnakeState
    {
        Alive = 0,
        Dead
    }
    
    private Vector2 MovementDirection;
    private Vector2 OldHeadPosition;

    private List<GameObject> TailPieces;
    private Stack<GameObject> UnusedTailPieces;

    private double TimeSinceMove;

    private string FailurePoint;

    private SnakeState CurrentState;

    [SerializeField] GameObject SnakeTailPrefab;

	// Use this for initialization
	void Start () {
        MovementDirection = Vector2.right;

        TailPieces = new List<GameObject>();
        UnusedTailPieces = new Stack<GameObject>();

        ResetSnake();

        CurrentState = SnakeState.Dead;
    }
	
	// Update is called once per frame
	void Update () {

        HandleInput();

        TimeSinceMove += Time.deltaTime;
        if(TimeSinceMove > (DEFAULT_MOVE_TIME / GetSize()))
        {
            MoveSnake();
            TimeSinceMove = 0d;
        }
    }
    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (MovementDirection == Vector2.down) return;
            MovementDirection = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (MovementDirection == Vector2.up) return;
            MovementDirection = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (MovementDirection == Vector2.right) return;
            MovementDirection = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (MovementDirection == Vector2.left) return;
            MovementDirection = Vector2.right;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Food"))
        {
            AddTailToSnake();
            collision.gameObject.SetActive(false);
        }
        else
        {
            KillSnake(collision.tag);
        }
    }

    public void ResetSnake()
    {
        MovementDirection = Vector2.up;

        transform.position = Vector3.zero;
        OldHeadPosition = Vector2.zero;

        RemoveTailFromSnake();

        CurrentState = SnakeState.Alive;

        TimeSinceMove = 0d;

        FailurePoint = "";
    }

    private void KillSnake(string failurePoint)
    {
        CurrentState = SnakeState.Dead;
        FailurePoint = failurePoint;
    }

    private void MoveSnake()
    {
        if (CurrentState == SnakeState.Dead) return;

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
        GameObject tailPiece;

        if (UnusedTailPieces.Count > 0) tailPiece = UnusedTailPieces.Pop();
        else tailPiece = Instantiate(SnakeTailPrefab);

        tailPiece.SetActive(true);

        tailPiece.transform.position = OldHeadPosition;
        tailPiece.transform.rotation = Quaternion.identity;

        TailPieces.Insert(0, tailPiece);
    }
    private void RemoveTailFromSnake()
    {
        for (int i = 0; i < TailPieces.Count; i++)
        {
            GameObject tailPiece = TailPieces[i];
            tailPiece.SetActive(false);
            UnusedTailPieces.Push(tailPiece);
        }

        TailPieces.Clear();
    }

    public int GetSize()
    {
        return TailPieces.Count + 1;
    }
    public SnakeState GetSnakeState()
    {
        return CurrentState;
    }
    public string GetFailurePoint()
    {
        return FailurePoint;
    }
}
