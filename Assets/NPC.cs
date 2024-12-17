using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    public Animator animator; // Reference to the Animator
    public float range = 10f; // Range around the character to find random points
    public int coolDown;
    public bool isMoving = false;

    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");

    void Start()
    {
        MoveToRandomPoint();
        coolDown = 200;
    }

    void Update()
    {
        isMoving = agent.velocity.magnitude > 0.1f;
        // Check if the agent has reached its destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && coolDown == 0 )
        {
            MoveToRandomPoint();
            coolDown = 200;
        }else{

            if(coolDown > 0)
            {
                coolDown -= 1;
            }
        }

        
        animator.SetBool(IsWalkingHash, isMoving);
    }

    void MoveToRandomPoint()
    {
        // Generate a random point within the specified range
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Ensure it stays on the same plane

        // Find the nearest point on the NavMesh
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
