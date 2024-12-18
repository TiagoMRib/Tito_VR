using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
public class NPCAdopter : MonoBehaviour
{
    public string playerTag = "Player";  
    public Animator animator;             
    private NavMeshAgent navAgent;        
    private bool isTriggered = false;    

    public Transform destinationPoint;
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

        if (destinationPoint == null)
        {
            Debug.LogError("Destination point is not assigned to the NPCAdopter script.");
        }
        animator.SetBool("isWalking", false);
    }

    public void TriggerAdopter()
    {
        if (!isTriggered)
        {
            isTriggered = true;

            // Start walking animation
            if (animator != null)
            {
                animator.SetBool("isWalking", true);  
            }

            // Command the NavMeshAgent to move to the player's position
            if (navAgent != null)
            {
                navAgent.SetDestination(destinationPoint.position);
            }
        }
    }

    void Update()
    {
        if (isTriggered && navAgent != null && !navAgent.pathPending)
        {
            // Check if the adopter has reached the destination
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                ReachPlayer();
            }
        }
    }

    private void ReachPlayer()
    {
        if (animator != null)
        {
            // Stop walking animation and trigger the knelling animation
            animator.SetBool("isWalking", false);
            animator.SetBool("knell", true); 
        }

        // Delay ending the game by 3 seconds
        StartCoroutine(PickUpPlayer());
    }

    private IEnumerator PickUpPlayer()
    {
        Debug.Log("NPCAdopter is picking up the player. Waiting for 3 seconds...");
        
        // Wait for 3 seconds before ending the game
        yield return new WaitForSeconds(3f);
        
        EndGame();
    }

    private void EndGame()
    {
        // Load new scene
        Debug.Log("Game Over: Player has been picked up!");
        SceneManager.LoadScene("MainMenu");
    }
}