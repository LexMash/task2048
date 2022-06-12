using UnityEngine;
using UnityEngine.UI;
using System;

public class Interface : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Text _scorePanel;
    [SerializeField] private Text _endGameText;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;

    public event Action Restarting;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(Restart);
    }
    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        Restarting?.Invoke();
        _endGameText.gameObject.SetActive(false);
        ScoreView(0);
    }

    public void ScoreView(int score)
    {
        _scorePanel.text = score.ToString();
    }

    public void EndGameView(bool isGameOver, bool isWin)
    {
        if (isGameOver)
        {
            _endGameText.gameObject.SetActive(true);
            _endGameText.color = _loseColor;
            _endGameText.text = "You lose!";
        }

        if (isWin)
        {
            _endGameText.gameObject.SetActive(true);
            _endGameText.color = _winColor;
            _endGameText.text = "You win!";
        }
    }
}
