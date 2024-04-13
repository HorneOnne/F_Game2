using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }
    public static event System.Action OnEventTimeReached;

    private float _timer = 300f; // 5 minutes in seconds
    private bool _startTimer = false;

  
    private float _eventInterval = 20f; // Interval in seconds
    private float _elapsedTime = 0f; // Time elapsed since last interval


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameplayManager.Instance.CurrentState != GameplayManager.GameState.PLAYING) return;
  
        if (_startTimer)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                //Debug.Log(timer);
            }
            else
            {
                // Timer has reached zero, handle the end of the countdown here
                _timer = 0f; // Reset the timer to zero or clamp it to prevent negative values
                _startTimer = false; // Stop the timer

                GameplayManager.Instance.ChangeGameState(GameplayManager.GameState.GAMEOVER);
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _eventInterval)
            {
                _elapsedTime -= _eventInterval;
                OnEventTimeReached.Invoke();
            }
        }
    }

    public string TimeToText()
    {
        int minutes = Mathf.FloorToInt(_timer / 60f);
        int seconds = Mathf.FloorToInt(_timer % 60f);
        return $"{minutes:D2}:{seconds:D2}";
    }

    public void StartTimer()
    {
        _startTimer = true;
    }

    public void StopTimer()
    {
        _startTimer = false;
    }
}