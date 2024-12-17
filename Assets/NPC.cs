using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class NPC : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    public Animator animator; // Reference to the Animator
    public float range = 10f; // Range around the character to find random points
    public int coolDown;
    public bool isMoving = false;

    public float knockbackForce = 5f; // Knockback force applied to the player
    public AudioClip shoutSound; // Sound to play when player is near
    private AudioSource audioSource; // Reference to the AudioSource

    private bool isReacting = false;
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int NearPlayerHash = Animator.StringToHash("playerNear");

    void Start()
    {
        MoveToRandomPoint();
        coolDown = 200;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isReacting) return;
        isMoving = agent.velocity.magnitude > 0.1f;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && coolDown == 0)
        {
            MoveToRandomPoint();
            coolDown = 200;
        }
        else
        {
            if (coolDown > 0)
            {
                coolDown -= 1;
            }
        }

        animator.SetBool(IsWalkingHash, isMoving);
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !isReacting)
        {
            StartCoroutine(ReactToPlayer(other.transform));
        }
    }

    private IEnumerator ReactToPlayer(Transform player)
    {
        isReacting = true; // Pause NPC behavior
        
        // Stop the agent from moving
        agent.isStopped = true;

        // Turn toward the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0; // Ensure rotation happens only on the horizontal plane

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);


        // Snap to final rotation to ensure perfect alignment
        transform.rotation = lookRotation;

        animator.SetBool(NearPlayerHash, isReacting);
        // Play the shout sound
        if (shoutSound != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(shoutSound);
        }

        // Apply knockback to the player
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 knockbackDirection = (player.position - transform.position).normalized;
            knockbackDirection.y = 0; // Ensure the knockback is horizontal
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }

        
        yield return new WaitForSeconds(2.5f);

        // Resume NPC behavior
        agent.isStopped = false;
        isReacting = false;
        animator.SetBool(NearPlayerHash, isReacting);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection radius in the editor
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider && collider.isTrigger)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, collider.radius);
        }
    }
}
