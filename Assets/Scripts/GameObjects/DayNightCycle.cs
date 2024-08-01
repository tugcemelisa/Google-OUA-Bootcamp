using System.Collections;
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
    [SerializeField] private float transitionDuration = 10f;
    private float fixedDayDuration;

    private float time;
    private float blendTime;

    private void OnEnable()
    {
        SittingArea.OnPlayerSit += StartTransitionToNight;
        WolfManager.OnHuntOver += StartTransitionToDay;
    }
    private void OnDisable()
    {
        SittingArea.OnPlayerSit -= StartTransitionToNight;
        WolfManager.OnHuntOver -= StartTransitionToDay;
    }

    private void Start()
    {
        executingState = CycleState.Normal;
        SetSkybox(daySkybox, nightSkybox);
        RenderSettings.skybox.SetFloat("_Blend", 0);
        fixedDayDuration = dayDuration;
    }

    void Update()
    {
        time += Time.deltaTime / dayDuration;
        blendTime += Time.deltaTime*2 / dayDuration;

        switch (executingState)
        {
            case CycleState.Normal:
                DoNormalCycle();
                break;
            case CycleState.TransitionToNight:
                DoNightTransition();
                break;
            case CycleState.TransitionToDay:
                DoDayTransition();
                break;
        }

        UpdateLighting(time);
        LerpSkybox();
    }

    void UpdateLighting(float timePercent)
    {
        directionalLight.transform.Rotate(Vector3.up, (1f/ 5400f)*360f, Space.World);
        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);
    }

    private void StartTransitionToNight()
    {
        dayDuration = transitionDuration;
        executingState = CycleState.TransitionToNight;
    }
    private void StartTransitionToDay()
    {
        dayDuration = transitionDuration;
        executingState = CycleState.TransitionToDay;
    }

    private void DoNormalCycle()
    {
        if (time >= 1f)
        {
            SetSkybox(daySkybox, nightSkybox);
            blendTime = 0f;
            time = 0f;
        }
    }

    private void DoNightTransition()
    {
        if (time >= 0.5f)
        {
            dayDuration = fixedDayDuration;
            SetSkybox(nightSkybox, daySkybox);
            blendTime = 0f;
            //skyboxTime = -1;
            //RenderSettings.skybox.SetTexture("_Cubemap1", nightSkybox);
            executingState = CycleState.Normal;
        }
    }

    private void DoDayTransition()
    {
        if (time >= 1f)
        {
            dayDuration = fixedDayDuration;
            SetSkybox(daySkybox, nightSkybox);
            blendTime = 0f;
            time = 0f;
            executingState = CycleState.Normal;
        }
    }

    [SerializeField] private Cubemap daySkybox;
    [SerializeField] private Cubemap nightSkybox;

    //    RenderSettings.skybox.SetTexture("_Cubemap1", b);
    //}

    private void LerpSkybox()
    {
        RenderSettings.skybox.SetFloat("_Blend", blendTime);
    }

    private void SetSkybox(Cubemap a, Cubemap b)
    {
        RenderSettings.skybox.SetTexture("_Cubemap1", a);
        RenderSettings.skybox.SetTexture("_Cubemap2", b);
    }
}
