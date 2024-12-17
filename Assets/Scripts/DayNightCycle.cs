using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;
    public Light moonLight;
    public Transform player; // Reference to the player's transform
    public float fullDayLength = 24f; // Length of a full day in seconds
    public float timeScale = 1f; // Speed multiplier for time progression

    public float currentTime = 0f; // Current time in the day cycle
    public float timePerCycle = 24f; // 24 hours in a cycle
    private float targetTime = -1f; // Target time for smooth skipping
    private float skipSpeedMultiplier = 10f; // Speed multiplier during skip

    // Ellipse parameters
    public float horizontalRadius = 120f; // Horizontal radius of the ellipse
    public float verticalRadius = 70f; // Vertical radius of the ellipse
    public float offsetX = 30f; // Horizontal offset for the sun's path

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

        // Update sun and moon positions
        UpdateLighting();
    }

    void UpdateLighting()
    {
        // Normalize time (0.0 to 1.0) for the cycle
        float normalizedTime = currentTime / timePerCycle;

        // Calculate sun's position on the ellipse
        Vector3 sunPosition = CalculateEllipsePosition(normalizedTime, true);

        // Update sun's light position and rotation
        sunLight.transform.position = sunPosition;
        if (player != null) sunLight.transform.LookAt(player.position);

        // Calculate moon's position on the opposite side of the ellipse
        Vector3 moonPosition = CalculateEllipsePosition((normalizedTime + 0.5f) % 1f, false);

        // Update moon's light position and rotation
        moonLight.transform.position = moonPosition;
        if (player != null) moonLight.transform.LookAt(player.position);

        // Adjust lighting intensity based on time of day
        AdjustLighting(normalizedTime);
    }

    Vector3 CalculateEllipsePosition(float time, bool isSun)
    {
        // Parametric equation for an ellipse
        float angle = time * Mathf.PI * 2f; // Full circle (0 to 2π)
        float x = offsetX;
        float y = Mathf.Sin(angle) * verticalRadius;
        float z = Mathf.Cos(angle) * horizontalRadius;

        return new Vector3(isSun ? x : -x, y, z); // Moon is opposite to sun
    }

    void AdjustLighting(float normalizedTime)
    {
        // Calculate sun and moon intensities based on the normalized time
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

        // Update lighting during the skip
        UpdateLighting();
    }

    public void SkipDaysSmooth(float days)
    {
        // Set the target time based on the number of days to skip
        targetTime = (currentTime + (days * timePerCycle)) % timePerCycle;
    }
}
