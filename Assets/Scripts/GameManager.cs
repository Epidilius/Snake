using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private enum GameScreens
    {
        FirstStart = 0,
        Game,
        GameOver
    }

    private const string SCORE_LENGTH_PREFIX = "Length: ";
    private const string SCORE_TIME_PREFIX = "Time: ";

    [SerializeField] GameObject Snake;

    private double CurrentGameTime;

    private GameScreens CurrentScreen;

    private bool ShouldUpdateScore;

    private List<int> ScoreTailLength;
    private List<double> ScoreTime;

    [SerializeField] private Canvas GameCanvas;
    [SerializeField] private Canvas GameOverCanvas;
    [SerializeField] private Canvas FirstStartCanvas;

    [SerializeField] private Text ScoreLengthText;
    [SerializeField] private Text ScoreTimeText;

    [SerializeField] private Text GameOverFailureText;
    [SerializeField] private Text HighScoreLengthText;
    [SerializeField] private Text HighScoreTimeText;
    [SerializeField] private Text GameOverLengthText;
    [SerializeField] private Text GameOverTimeText;

    // Use this for initialization
    void Start () {
        ScoreTailLength = new List<int>();
        ScoreTime = new List<double>();

        CurrentGameTime = 0d;

        CurrentScreen = GameScreens.FirstStart;
        
        ShouldUpdateScore = false;

        GameCanvas.enabled = false;
        GameOverCanvas.enabled = false;

        InvokeRepeating("UpdateScore", 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
        Snake.SnakeState currentSnakeState = Snake.GetComponent<Snake>().GetSnakeState();

        if (currentSnakeState == global::Snake.SnakeState.Dead || ShouldUpdateScore == false)
        {
            if(CurrentScreen == GameScreens.Game)
            {
                InitiateGameOver();
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                StartNewGame();
            }
        }
        else if(currentSnakeState == global::Snake.SnakeState.Alive)
        {
            int tailLength = Snake.GetComponent<Snake>().GetSize();
            GetComponent<FoodSpawner>().SetSnakeLength(tailLength);

            CurrentGameTime += Time.deltaTime;
        }
	}

    private void StartNewGame()
    {
        if(CurrentScreen == GameScreens.FirstStart)
        {
            FirstStartCanvas.enabled = false;
        }

        CurrentScreen = GameScreens.Game;

        CurrentGameTime = 0d;
        ShouldUpdateScore = true;

        GetComponent<FoodSpawner>().ResetFoodSpawner();
        Snake.GetComponent<Snake>().ResetSnake();

        DisplayRegularUI();
        UpdateScore();
    }
    private void InitiateGameOver()
    {
        string failurePoint = Snake.GetComponent<Snake>().GetFailurePoint();
        CurrentScreen = GameScreens.GameOver;

        int tailLength = Snake.GetComponent<Snake>().GetSize();
        
        GetComponent<FoodSpawner>().StopSpawningFood();

        UpdateHighScore(tailLength);
        DisplayGameOverUI(failurePoint, tailLength);
    }

    private void DisplayRegularUI()
    {
        GameCanvas.enabled = true;
        GameOverCanvas.enabled = false;
    }
    private void DisplayGameOverUI(string failurePoint, int tailLength)
    {
        ShouldUpdateScore = false;

        GameCanvas.enabled = false;
        GameOverCanvas.enabled = true;

        GameOverFailureText.text = "You Hit: " + failurePoint;
        GameOverLengthText.text  = "Your Score: " + tailLength;
        GameOverTimeText.text    = "Your Time: " + (int)CurrentGameTime;
        
        HighScoreLengthText.text = "High Score: " + ScoreTailLength[0];
        HighScoreTimeText.text   = "Longest Time: " + (int)ScoreTime[0];
    }

    private void UpdateScore()
    {
        if (ShouldUpdateScore == false) return;

        int tailLength = Snake.GetComponent<Snake>().GetSize();

        ScoreLengthText.text = SCORE_LENGTH_PREFIX + tailLength;
        ScoreTimeText.text = SCORE_TIME_PREFIX + (int)CurrentGameTime;
    }
    private void UpdateHighScore(int tailLength)
    {
        UpdateScore();

        ScoreTailLength.Add(tailLength);
        ScoreTime.Add(CurrentGameTime);

        ScoreTailLength.Sort();
        ScoreTime.Sort();

        ScoreTailLength.Reverse();
        ScoreTime.Reverse();
    }
    private bool IsCurrentScoreHighScore()
    {
        if (ScoreTailLength.Count == 0) return true;

        if(Snake.GetComponent<Snake>().GetSize() > ScoreTailLength[0])
        {
            return true;
        }

        return false;
    }
    private bool IsLongestTime()
    {
        if (ScoreTime.Count == 0) return true;

        if(CurrentGameTime > ScoreTime[0])
        {
            return true;
        }

        return false;
    }
}
