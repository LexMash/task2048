using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RectTransform _tilesGrid;
    [SerializeField] private Interface _interface;
    [SerializeField] private SwipeSystem _swipeSystem;
    [SerializeField] private TileConfig _tileConfig;

    private Tile[] _allTilesOnGrid;
    private Field _field;
   
    private bool _isGameOver;
    private bool _isWin;

    private int _score;

    private void Awake()
    {
        _allTilesOnGrid = GetComponentsInChildren<Tile>();

        _field = new Field(4, _allTilesOnGrid, _tileConfig);
    }

    private void OnEnable()
    {
        _swipeSystem.MoveUp += MoveUp;
        _swipeSystem.MoveDown += MoveDown;
        _swipeSystem.MoveLeft += MoveLeft;
        _swipeSystem.MoveRight += MoveRight;

        _interface.Restarting += Restarting;

        _field.IsWon += CheckGameEnd;
        _field.ScoreAwarded += ScoreAwarded;
    }

    private void OnDisable()
    {
        _swipeSystem.MoveUp -= MoveUp;
        _swipeSystem.MoveDown -= MoveDown;
        _swipeSystem.MoveLeft -= MoveLeft;
        _swipeSystem.MoveRight -= MoveRight;

        _interface.Restarting -= Restarting;

        _field.IsWon -= CheckGameEnd;
        _field.ScoreAwarded -= ScoreAwarded;
    }

    private void Start()
    {
        _isGameOver = false;
        _isWin = false;

        Restarting();      
    }

    public void Restarting()
    {
        _isGameOver = false;
        _isWin = false;

        _score = 0;
        _field.Refill();
        _swipeSystem.Enable(_isGameOver, _isWin);
    }

    private void CheckGameEnd(bool isWin = false)
    {
        _isGameOver = _field.CheckField();
        _isWin = isWin;

        _interface.EndGameView(_isGameOver, _isWin);
        _swipeSystem.Enable(_isGameOver, _isWin);
    }

    private void ScoreAwarded(int scorePoint)
    {
        Scoring(scorePoint);
        _interface.ScoreView(_score);
    }
    private void Scoring(int scorePoint)
    {
        _score += scorePoint;        
    }

    private void MoveUp() 
    {
        _field.MoveUp();
        CheckGameEnd();
    }

    private void MoveDown()
    {
        _field.MoveDown();
        CheckGameEnd();
    }

    private void MoveRight()
    {
        _field.MoveRight();
        CheckGameEnd();
    }

    private void MoveLeft()
    {
        _field.MoveLeft();
        CheckGameEnd();
    }
}
