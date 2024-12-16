using UnityEngine;

public class SmellTrigger : MonoBehaviour
{
    public ParticleSystem initialParticles; // Reference to initial particle system
    public GameObject nextPoint; // Reference to the next point in the sequence
    public float delayBeforeNextPoint = 1.0f; // Delay before activating the next point
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // Ensure it only triggers once
            TriggerNextPoint();
        }
    }

    private void TriggerNextPoint()
    {
        // Activate the next point after a delay
        if (nextPoint != null)
        {
            StartCoroutine(ActivateNextPoint());
        }
    }

    private System.Collections.IEnumerator ActivateNextPoint()
    {
        yield return new WaitForSeconds(delayBeforeNextPoint);
        nextPoint.SetActive(true);
        Debug.Log("Next point activated.");
        if (initialParticles != null)
        {
            initialParticles.Stop();
            Debug.Log("Point triggered, stopping particles.");
        }
    }
}
