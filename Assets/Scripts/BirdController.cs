using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Animator animator; // Reference to bird Animator
    public Transform[] flightPoints; // Set of points to fly toward
    public float flySpeed = 5f; // Speed of flight
    public float turnSpeed = 5f;

    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip idleClip; // Idle sound clip
    public AudioClip flyClip;  // Fly sound clip

    private bool isFlying = false;
    private int currentFlightPoint = 0;

    private float idleTimer = 0f; // Timer for idle animations
    private float idleInterval = 5f; // Average interval for switching to "eat" animation

    void Start()
    {
        // Set a random interval for the first idle-to-eat transition
        idleInterval = Random.Range(3f, 7f);
        
        // Play idle sound when bird starts idle
        PlayIdleSound();
    }

    void Update()
    {
        if (!isFlying)
        {
            HandleIdleBehavior();
        }
    }

    void HandleIdleBehavior()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleInterval)
        {
            // Trigger the eat animation and play eat sound
            animator.SetTrigger("Eat");
            // Reset the timer and pick a new random interval
            idleTimer = 0f;
            idleInterval = Random.Range(3f, 7f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFlying)
        {
            isFlying = true;
            animator.SetTrigger("Fly"); // Start flying animation
            PlayFlySound();
            StartFlying();
        }
    }

    void StartFlying()
    {
        // Fly to each waypoint in sequence
        if (currentFlightPoint < flightPoints.Length)
        {
            Vector3 target = flightPoints[currentFlightPoint].position;
            StartCoroutine(FlyToPoint(target));
            currentFlightPoint++;
        }
    }

    System.Collections.IEnumerator FlyToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);

            // Calculate the direction to the target
            Vector3 direction = (target - transform.position).normalized;

            // Rotate smoothly toward the target direction
            if (direction != Vector3.zero) // Prevent unnecessary rotations
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            yield return null;
        }

        if (currentFlightPoint < flightPoints.Length)
        {
            StartFlying(); // Move to the next point
        }
        else
        {
            Destroy(gameObject); // Birds vanish after flying away
        }
    }

    // Play the idle sound when the bird starts idling
    void PlayIdleSound()
    {
        if (audioSource && idleClip)
        {
            audioSource.clip = idleClip;
            audioSource.loop = true; // Loop idle sound
            audioSource.Play();
        }
    }

    // Play the flying sound when the bird starts flying
    void PlayFlySound()
    {
        if (audioSource && flyClip)
        {
            audioSource.clip = flyClip;
            audioSource.loop = false; // Flying sound is usually one-off
            audioSource.Play();
        }
    }

    
}
