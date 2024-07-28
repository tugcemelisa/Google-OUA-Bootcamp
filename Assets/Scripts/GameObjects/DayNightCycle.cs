using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light directionalLight; 
    public Gradient lightColor; 
    public AnimationCurve lightIntensity; 
    public float dayDuration = 120f;
    //private float dayDurationAsSeconds;

    private float time;

    //private void Start()
    //{
    //    dayDurationAsSeconds *= 60;
    //}

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
        directionalLight.transform.Rotate(Vector3.up, (1f/ 5400f)*360f, Space.World);
        directionalLight.color = lightColor.Evaluate(timePercent);
        directionalLight.intensity = lightIntensity.Evaluate(timePercent);
    }
}
