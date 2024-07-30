using UnityEngine;

enum CycleState
{
    Normal,
    TransitionToNight,
    TransitionToDay
}

public class DayNightCycle : MonoBehaviour
{
    CycleState executingState;
    public Light directionalLight; 
    public Gradient lightColor; 
    public AnimationCurve lightIntensity; 
    public float dayDuration = 120f;

    private float time;

    private void OnEnable()
    {
        SittingArea.OnPlayerSit += StartTransitionToNight;
        WolfManager.OnHuntOver += StartTransitionToNight;
    }
    private void OnDisable()
    {
        SittingArea.OnPlayerSit -= StartTransitionToNight;
        WolfManager.OnHuntOver -= StartTransitionToNight;
    }

    private void Start()
    {
        executingState = CycleState.Normal;
    }

    void Update()
    {
        time += Time.deltaTime / dayDuration;

        switch (executingState)
        {
            case CycleState.Normal:
                DoNormalCycle();
                break;
            case CycleState.TransitionToNight:
                DoNightTransition();
                break;
        }

        UpdateLighting(time);
    }

    void UpdateLighting(float timePercent)
    {
        directionalLight.transform.Rotate(Vector3.up, (1f/ 5400f)*360f, Space.World);
        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);
    }

    private void StartTransitionToNight()
    {
        dayDuration /= 36f;
        executingState = CycleState.TransitionToNight;
    }

    private void DoNormalCycle()
    {
        if (time >= 1f)
        {
            time = 0f;
        }
    }

    private void DoNightTransition()
    {
        if (time >= 0.5f)
        {
            dayDuration = 360f;
            executingState = CycleState.Normal;
        }
    }
}
