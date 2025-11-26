using System.Collections;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    // This method is called whenever you want to start a countdown timer.
    // 'duration' = how long the timer should run in seconds.
    // 'onTimerComplete' = the method (callback) to call when the timer finishes.
    public void StartTimer(float duration, System.Action onTimerComplete)
    {
        Debug.Log("TimerManager StartTimer called with duration: " + duration);

        // StartCoroutine tells Unity to "pause and continue later."
        // It runs the TimerCoroutine in the background while the game continues.
        StartCoroutine(TimerCoroutine(duration, onTimerComplete));
    }

    // This is the coroutine that actually waits for the specified time.
    private IEnumerator TimerCoroutine(float duration, System.Action onTimerComplete)
    {
        Debug.Log("TimerCoroutine started with duration: " + duration);

        // 'yield return new WaitForSeconds(duration)' means:
        // "Pause this method here, and continue after 'duration' seconds have passed."
        // Meanwhile, the rest of the game keeps running as normal.
        yield return new WaitForSeconds(duration);

        Debug.Log("TimerCoroutine completed");

        // After waiting is done, check if there's a method to call (not null).
        // If yes, call it — this signals that the timer has finished.
        onTimerComplete?.Invoke();
    }
}
