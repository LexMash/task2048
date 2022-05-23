using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform _tilesGrid;
    [SerializeField] private Text _endGameText;
    [SerializeField] private Text _scorePanel;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;
    [SerializeField] private float _swipeOffset;

    private Tile[] _allTilesOnGrid;
    private Field _field;
   
    private bool _isGameOver;
    private bool _isWin;

    private int _score;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 swipeDirection;

    private void Awake()
    {
        _allTilesOnGrid = GetComponentsInChildren<Tile>();

        _field = new Field(4, _allTilesOnGrid);
    }

    private void OnEnable()
    {
        _field.IsWon += CheckGameEnd;
        _field.ScoreAwarded += CountScore;
    }

    private void OnDisable()
    {
        _field.IsWon -= CheckGameEnd;
        _field.ScoreAwarded -= CountScore;
    }

    private void Start()
    {
        _isGameOver = false;
        _isWin = false;

        StartNewGame();      
    }

    public void StartNewGame()
    {
        _isGameOver = false;
        _isWin = false;
        _endGameText.gameObject.SetActive(false);

        _score = 0;
        _scorePanel.text = _score.ToString();

        _field.Refill(_field);
    }

    private void CheckGameEnd(bool isWin)
    {
        _isGameOver = _field.CheckField(_field);
        _isWin = isWin;

        if (_isGameOver)
        {
            _endGameText.gameObject.SetActive(true);
            _endGameText.color = _loseColor;
            _endGameText.text = "You lose!";
        }

        if (_isWin)
        {
            _endGameText.gameObject.SetActive(true);
            _endGameText.color = _winColor;
            _endGameText.text = "You win!";
        }
    }

    private void CountScore(int scorePoint)
    {
        _score += scorePoint;
        _scorePanel.text = _score.ToString();
    }

    private void Update()
    {
        if (!_isGameOver && !_isWin)
        {
            Swipe();

            InputProcess();
        }
    }

    private void Swipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                swipeDirection = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                /*swipeDirection = swipeDirection.normalized;*/
                /*print(swipeDistance);*/

                //normalize the 2d vector
                swipeDirection.Normalize();


                //swipe upwards
                if (swipeDirection.y > 0.5 && (swipeDirection.x > -_swipeOffset || swipeDirection.x < _swipeOffset))
                {
                    MoveUp();
                    CheckGameEnd(false);

                    Debug.Log("up swipe");
                }   

                //swipe down
                if (swipeDirection.y < -0.5 && (swipeDirection.x > -_swipeOffset || swipeDirection.x < _swipeOffset))
                {
                    MoveDown();
                    CheckGameEnd(false);

                    Debug.Log("down swipe");
                }

                //swipe left
                if (swipeDirection.x < -0.5 && (swipeDirection.y > -_swipeOffset && swipeDirection.y < _swipeOffset))
                {
                    MoveLeft();
                    CheckGameEnd(false);

                    Debug.Log("left swipe");
                }

                //swipe right
                if (swipeDirection.x > 0.5 && (swipeDirection.y > -_swipeOffset && swipeDirection.y < _swipeOffset))
                {
                    MoveRight();
                    CheckGameEnd(false);

                    Debug.Log("right swipe");
                }
            }
        }
    }

    private void InputProcess()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveUp();
            CheckGameEnd(false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveDown();
            CheckGameEnd(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRight();
            CheckGameEnd(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeft();
            CheckGameEnd(false);
        }
    }

    private void MoveUp() 
    {
        _field.MoveUp(_field);
    }

    private void MoveDown()
    {
        _field.MoveDown(_field);
    }

    private void MoveRight()
    {
        _field.MoveRight(_field);
    }

    private void MoveLeft()
    {
        _field.MoveLeft(_field);
    }
}
