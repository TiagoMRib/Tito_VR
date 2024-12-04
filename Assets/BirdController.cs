using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Animator animator; // Reference to bird Animator
    public Transform[] flightPoints; // Set of points to fly toward
    public float flySpeed = 5f; // Speed of flight
    public float turnSpeed = 5f; // Speed of rotation

    private bool isFlying = false;
    private int currentFlightPoint = 0;

    private float idleTimer = 0f; // Timer for idle animations
    private float idleInterval = 5f; // Average interval for switching to "eat" animation

    void Start()
    {
        // Set a random interval for the first idle-to-eat transition
        idleInterval = Random.Range(3f, 7f);
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
            // Trigger the eat animation
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
            // Move toward the target
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
}
