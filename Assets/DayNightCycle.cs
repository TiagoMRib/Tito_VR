using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight; 
    public Light moonLight; 
    public float fullDayLength = 24f; // Length of a full day in seconds
    public float timeScale = 1f; // Speed multiplier for time progression

    private float currentTime = 0f; // Current time in the day cycle
    private float timePerCycle = 24f; // 24 hours in a cycle

    private float targetTime = -1f; // Target time for smooth skipping
    private float skipSpeedMultiplier = 10f; // Speed multiplier during skip

    void Update()
    {
        // Determine if we are skipping
        if (targetTime >= 0f)
        {
            SmoothSkip();
        }
        else
        {
            // Normal time progression
            currentTime += (Time.deltaTime / fullDayLength) * timeScale * timePerCycle;

            if (currentTime >= timePerCycle)
                currentTime -= timePerCycle;
        }

        // Update rotations
        UpdateLighting();
    }

    void UpdateLighting()
    {
        // Calculate the rotation angle based on the current time
        float sunRotationAngle = (currentTime / timePerCycle) * 360f; // Full 360° rotation over one cycle

        // Rotate sun and moon around the X-axis for a realistic day-night arc
        sunLight.transform.rotation = Quaternion.Euler(sunRotationAngle - 90f, 0f, 15f);
        moonLight.transform.rotation = Quaternion.Euler(sunRotationAngle + 90f, 0f, 15f);

        // Optional: Adjust lighting intensity based on the angle
        AdjustLighting(currentTime / timePerCycle);
    }


    void AdjustLighting(float normalizedTime)
    {
        float sunIntensity = Mathf.Clamp01(Mathf.Cos(normalizedTime * Mathf.PI * 2));
        float moonIntensity = 1f - sunIntensity;

        sunLight.intensity = sunIntensity * 1.5f;
        moonLight.intensity = moonIntensity * 0.5f;
    }

    void SmoothSkip()
    {
        // Smoothly move the current time towards the target
        float step = (Time.deltaTime / fullDayLength) * skipSpeedMultiplier * timePerCycle;
        currentTime = Mathf.MoveTowards(currentTime, targetTime, step);

        // Stop skipping if we've reached the target
        if (Mathf.Approximately(currentTime, targetTime))
        {
            targetTime = -1f; // Reset target
        }

        // Loop time correctly if needed
        if (currentTime >= timePerCycle)
            currentTime -= timePerCycle;

        // Update light rotations during the skip
        UpdateLighting();
    }


    public void SkipDaysSmooth(float days)
    {
        // Set the target time based on the number of days to skip
        targetTime = (currentTime + (days * timePerCycle)) % timePerCycle;
    }
}
