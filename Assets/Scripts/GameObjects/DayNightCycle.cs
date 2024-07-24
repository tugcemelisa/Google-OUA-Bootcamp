using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight; 
    public Gradient lightColor; 
    public AnimationCurve lightIntensity; 
    public float dayDuration = 60f; 

    private float time;

    void Update()
    {
        time += Time.deltaTime / dayDuration;
        if (time >= 1f)
        {
            time = 0f;
        }

        UpdateLighting(time);
    }

    void UpdateLighting(float timePercent)
    {
        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);
    }
}
