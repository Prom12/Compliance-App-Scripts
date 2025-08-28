using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoMTimerController : MonoBehaviour
{
    [SerializeField] public GeneralEnvironmentTimer GeneralEnvironmentTimer;
    public bool isTimerRunning = false;
    // private float timerDuration = 60f; // 1 minute in seconds
    // private float timerDuration = 420f; // 7 minutes in seconds
    private float timerDuration = 300f; // 5 minutes in seconds
    // private float timerDuration = 30f; // 30  seconds

    public float currentTime = 0f;
    public bool HasStopped = false;
    

    private void Update()
    {
        if (isTimerRunning)
        {
            // Update the timer
            currentTime += Time.deltaTime;

            // Check if the timer has reached its duration
            if (currentTime >= timerDuration)
            {
                // Timer is complete
                Debug.Log("Timer Complete!");
                // You can perform any actions you need here

                // Stop the timer
                StopTimer();
            }
        }
    }

    private IEnumerator TimerCoroutine()
    {
        isTimerRunning = true;
        GeneralEnvironmentTimer.toggleTimer(true);
        HasStopped = false;

        while (currentTime < timerDuration)
        {
            yield return null;
        }

        isTimerRunning = false;
    }

    public void StartTimer()
    {
        if (!isTimerRunning)
        {
            StartCoroutine(TimerCoroutine());

        }
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        currentTime = 0f;
        HasStopped = true;
        StopCoroutine(TimerCoroutine());
        // StopAllCoroutines();
    }

    // public void PausePlayTimer(bool isPaused)
    // {

    //     if (isTimerRunning && isPaused)
    //     {
    //         // Pause the timer
    //         isTimerRunning = false;
    //         GeneralEnvironmentTimer.toggleTimer(false);


    //     }
    //     else
    //     {
    //         // Resume the timer
    //         isTimerRunning = true;
    //         StartCoroutine(TimerCoroutine());
    //         GeneralEnvironmentTimer.toggleTimer(true);
            
    //     }
    // }

    public float GetStopTime(){
        return timerDuration;
    }
}
