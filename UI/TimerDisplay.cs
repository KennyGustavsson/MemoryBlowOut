using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerDisplay : MonoBehaviour
{
    private TextMeshProUGUI text = null;
    private Timer timer = null;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timer = GameManager.Instance.timer;

        text.text = GetTimeString();
    }

    private void OnEnable()
    {
        EventManager.onTimeUpdate += TimeUpdate;
    }

    private void OnDisable()
    {
        EventManager.onTimeUpdate -= TimeUpdate;
    }

    private void TimeUpdate(int time)
    {
        text.text = GetTimeString();
    }

    private string GetTimeString()
    {
        string minutes = "";
        string seconds = "";
        
        if (timer.minutes < 10)
        {
            minutes = "0" + timer.minutes.ToString();    
        }
        else
        {
            minutes = timer.minutes.ToString();    
        }
        
        if (timer.seconds < 10)
        {
            seconds = "0" + timer.seconds.ToString();    
        }
        else
        {
            seconds = timer.seconds.ToString();    
        }
        
        return $"{minutes} {seconds}";
    }
}
