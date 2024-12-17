using UnityEngine;
using UnityEngine.AI;

public class NPCAdopter : MonoBehaviour
{
    public string playerTag = "Player";  
    public Animator animator;             
    public AudioSource audioSource;     
    public AudioClip pickupSound;         

    private NavMeshAgent navAgent;        
    private bool isTriggered = false;    

    private GameObject player;            // Reference to the player object

    void Start()
    {
        // Ensure NavMeshAgent is attached to the adopter
        navAgent = GetComponent<NavMeshAgent>();
        if (navAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the adopter prefab.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned to the NPCAdopter script.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned to the NPCAdopter script.");
        }
    }

    public void TriggerAdopter()
    {
        if (!isTriggered)
        {
            isTriggered = true;

            // Start walking animation
            if (animator != null)
            {
                animator.SetBool("Walking", true);  
            }

            // Command the NavMeshAgent to move to the player's position
            if (navAgent != null)
            {
                player = GameObject.FindGameObjectWithTag(playerTag);  // Find the player object in the scene
                if (player != null)
                {
                    navAgent.SetDestination(player.transform.position);
                }
            }
        }
    }

    void Update()
    {
        if (isTriggered && navAgent != null && !navAgent.pathPending)
        {
            // Check if the adopter has reached the player
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                ReachPlayer();
            }
        }
    }

    private void ReachPlayer()
    {
        if (player == null) return;

        Debug.Log("NPCAdopter has reached the player.");

        // Stop walking animation
        if (animator != null)
        {
            animator.SetBool("Walking", false);

            // Trigger the picking up player animation
            animator.SetTrigger("PickUp"); 
        }

        // Play pickup sound
        if (audioSource != null && pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
        }

        PickUpPlayer();
    }

    private void PickUpPlayer()
    {
        
        Debug.Log("NPCAdopter is picking up the player. Game finished!");

        EndGame();
    }

    private void EndGame()
    {
        // Load new scene
        Debug.Log("Game Over: Player has been picked up!");
    }
}
