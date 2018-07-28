using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeController : MonoBehaviour
{
    public float slowdownFactor = 0.2f;
    public float slowdownSeconds = 0.5f;
    public ProgressbarController timeWarpBar;
    private bool isSlow = false;
    private float elapsedSlowdownSeconds = 0;
    private float defaultFixedDeltaTime;

    private void Awake()
    {
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start()
    {
        timeWarpBar.Initialize(50f);
    }

    void StartSlowMotion()
    {
        isSlow = true;
        timeWarpBar.Show();
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * defaultFixedDeltaTime;
        EventManager.TriggerEvent(E.OnTimeSlowDown);
    }

    void EndSlowMotion()
    {
        isSlow = false;
        timeWarpBar.Hide();
        elapsedSlowdownSeconds = 0;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * defaultFixedDeltaTime;
        EventManager.TriggerEvent(E.OnTimeSpeedRestore);
    }

    void UpdateTimeWarpBar()
    {
        var percentageLeft = 100 * (slowdownSeconds * slowdownFactor - elapsedSlowdownSeconds) /
                             (slowdownSeconds * slowdownFactor);
        timeWarpBar.SetProgressValue(percentageLeft);
    }

    void ExpireSlowMotion()
    {
        if (elapsedSlowdownSeconds >= slowdownSeconds * slowdownFactor)
        {
            EventManager.TriggerEvent(E.OnTimeWarpExpire);
            EndSlowMotion();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlowMotion();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndSlowMotion();
        }

        if (isSlow)
        {
            elapsedSlowdownSeconds += Time.deltaTime;
        }

        UpdateTimeWarpBar();
        ExpireSlowMotion();
    }
}