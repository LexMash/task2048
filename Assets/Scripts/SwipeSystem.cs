using UnityEngine;
using System;

public class SwipeSystem : MonoBehaviour
{
    [SerializeField] private float _swipeOffset;

    private bool _isEnable;

    public event Action MoveUp; 
    public event Action MoveDown;
    public event Action MoveLeft;
    public event Action MoveRight;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 swipeDirection;

    public void Enable(bool isGameOver, bool isWin)
    {
        if(isWin || isGameOver)
        {
            _isEnable = false;
        }

        _isEnable = true;
    }

    private void Update()
    {
        if (_isEnable)
        {
            Swipe();
        }
    }

    private void Swipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                secondPressPos = new Vector2(t.position.x, t.position.y);

                swipeDirection = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                swipeDirection.Normalize();

                if (swipeDirection.y > 0.5 && (swipeDirection.x > -_swipeOffset || swipeDirection.x < _swipeOffset))
                {
                    MoveUp?.Invoke();
                    Debug.Log("up swipe");
                }

                if (swipeDirection.y < -0.5 && (swipeDirection.x > -_swipeOffset || swipeDirection.x < _swipeOffset))
                {
                    MoveDown?.Invoke();
                    Debug.Log("down swipe");
                }

                if (swipeDirection.x < -0.5 && (swipeDirection.y > -_swipeOffset && swipeDirection.y < _swipeOffset))
                {
                    MoveLeft?.Invoke();
                    Debug.Log("left swipe");
                }

                if (swipeDirection.x > 0.5 && (swipeDirection.y > -_swipeOffset && swipeDirection.y < _swipeOffset))
                {
                    MoveRight?.Invoke();
                    Debug.Log("right swipe");
                }
            }
        }
    }
}
