using UnityEngine;

public class EnvironmentTriggerZone : MonoBehaviour
{
    public DayNightCycle dayNightCycle; 

    public float skipTime = 1f; // Time to skip in days. 1 = 24 hours

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered! Entered by: " + other.name);
        // Check if the object that entered the trigger zone is the player
        if (other.CompareTag("Player"))
        {
            StartTimeProgression();
        }
    }

    void StartTimeProgression()
    {
        dayNightCycle.SkipDaysSmooth(skipTime); 
    }
}
