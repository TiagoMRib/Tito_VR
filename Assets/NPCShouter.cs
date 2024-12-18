using UnityEngine;
using UnityEngine.AI;

public class NPCShouter : MonoBehaviour
{
    public string playerTag = "Player";   // Tag to identify the player
    public Transform destinationPoint;   // The point the NPC will walk to
    public Animator animator;            // Reference to the Animator
    public AudioSource audioSource;      // Reference to the AudioSource
    public AudioClip shoutSound;         // Shouting sound to play
    public float shoutDuration = 2f;     // Duration of the shout animation/sound

    private NavMeshAgent navAgent;       // NavMeshAgent for NPC movement
    private bool isTriggered = false;
    
    private bool isShouting = false;    // Tracks whether the NPC has been triggered

    void Start()
    {
        // Ensure NavMeshAgent is attached to the NPC
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the NPC.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the NPCShouter script.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned to the NPCShouter script.");
        }

        if (destinationPoint == null)
        {
            Debug.LogError("Destination point is not assigned to the NPCShouter script.");
        }
        animator.SetBool("isWalking", false);
    }

    public void TriggerShout()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            isShouting = true;
            // Start walking animation
            if (animator != null)
            {
                animator.SetBool("isWalking", true);
            }

            // Command the NavMeshAgent to move to the destination point
            if (navAgent != null && destinationPoint != null)
            {
                navAgent.SetDestination(destinationPoint.position);
            }
        }
    }

    void Update()
    {
        if (isTriggered && navAgent != null && !navAgent.pathPending)
        {
            // Check if the NPC has reached the destination
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                ReachDestination();
            }
        }
    }

    private void ReachDestination()
    {
        Debug.Log("NPC has reached the destination.");

        // Stop walking animation
        if (animator != null)
        {
            animator.SetBool("isWalking", false);

            // Trigger the shouting animation
            animator.SetBool("playerNear",true);
        }

        // Play shouting sound
        if (audioSource != null && shoutSound != null)
        {
            audioSource.PlayOneShot(shoutSound);
        }

        // Optional: Stop shouting after a duration
        Invoke(nameof(ResetNPC), shoutDuration);
    }

    private void ResetNPC()
    {
        isTriggered = false;
        animator.SetBool("playerNear",false);
        isShouting = false;
        
    }
}

