using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] public int minutes = default;
    [SerializeField] public int seconds = default;

    private bool isStarted = false;
    private bool isPaused = false;
    private bool atZeroTime = false;
    
    private void OnEnable()
    {
        EventManager.onPause += OnPause;
        EventManager.onStartTimer += OnStartTimer;
    }

    private void OnDisable()
    {
        EventManager.onPause -= OnPause;
        EventManager.onStartTimer -= OnStartTimer;
    }

    private void Awake()
    {
        SetClock();
    }

    private void OnStartTimer()
    {
        if(isStarted)
            return;
        
        StartCoroutine(Clock());
    }
    
    private IEnumerator Clock()
    {
        while (!atZeroTime)
        {
            yield return new WaitForSecondsRealtime(1f);
            UpdateClock();
        }
    }

    private void SetClock()
    {
        if (seconds > 59)
        {
            while (seconds > 59)
            {
                minutes++;
                seconds -= 60;
            }
        }
    }

    private void UpdateClock()
    {
        if(isPaused || atZeroTime)
            return;
        
        seconds--;
        if (seconds < 0)
        {
            seconds = 59;
            minutes--;
        }

        if (minutes <= 0 && seconds <= 0)
        {
            EventManager.TimeOutEvent();
            minutes = 0;
            seconds = 0;

            EventManager.TimeUpdate(0);
            
            atZeroTime = true;
            return;
        }

        EventManager.TimeUpdate(minutes * 60 + seconds);
    }

    private void OnPause(bool paused)
    {
        isPaused = paused;
    }
}