using UnityEngine;
using UnityEngine.AI;

public class NPCDog : MonoBehaviour
{
    public Transform targetPoint; // The point the dog will walk to
    public Animator animator;     // Reference to the Animator for the dog
    public AudioSource audioSource; // Reference to the AudioSource for barking
    public AudioClip barkingClip; // The barking sound clip
    public float barkingDuration = 5f; // Duration the dog will bark

    private NavMeshAgent navAgent; // NavMeshAgent for movement
    private bool isTriggered = false;

    private bool isBarking = false;

    void Start()
    {
        // Ensure NavMeshAgent is attached to the dog
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the dog prefab.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the NPCDog script.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned to the NPCDog script.");
        }
    }

    public void TriggerDog()
    {
        if (!isTriggered)
        {
            isTriggered = true;

            // Start walking animation
            if (animator != null)
            {
                animator.SetBool("Running", true); // Use the "Walking" parameter from the base Animator Controller
            }

            // Command the NavMeshAgent to move to the target point
            if (navAgent != null && targetPoint != null)
            {
                navAgent.SetDestination(targetPoint.position);
            }
        }
    }

    void Update()
    {
        if (isTriggered && navAgent != null && !navAgent.pathPending)
        {
            // Check if the dog has reached the destination
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                ReachTarget();
            }
        }
    }

private void ReachTarget()
{
    Debug.Log("NPCDog has reached the target point.");
    // Stop walking animation
    if (animator != null)
    {
        animator.SetBool("Running", false);

        // Trigger the angry barking animations
        animator.SetTrigger("Angry");
    }

    // Play barking sound
    if (audioSource != null && barkingClip != null && !isBarking)
    {
        isBarking = true;
        audioSource.clip = barkingClip;
        audioSource.Play();

        

    }
}


}
