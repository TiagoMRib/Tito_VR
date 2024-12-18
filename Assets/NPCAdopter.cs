using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
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
            Debug.LogError("Destination point is not assigned to the NPCShouter script.");
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
            Debug.Log("dentro");
            // Check if the adopter has reached the player
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                Debug.Log("reach");
                ReachPlayer();
            }
        }
    }

    private void ReachPlayer()
    {
        

        Debug.Log("NPCAdopter has reached the player.");

        // Stop walking animation
        if (animator != null)
        {
            animator.SetBool("isWalking", false);

            // Trigger the picking up player animation
            animator.SetBool("knell",true); 
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
        SceneManager.LoadScene("MainMenu");

    }
}
