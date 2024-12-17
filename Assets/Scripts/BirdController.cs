using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Animator animator; // Reference to bird Animator
    public Transform[] flightRoutes; // Array of potential flight routes (each route is a parent with waypoints)
    public float flySpeed = 5f; // Speed of flight
    public float turnSpeed = 5f;

    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip idleClip; // Idle sound clip
    public AudioClip flyClip;  // Fly sound clip

    private bool isFlying = false;
    private Transform[] currentRoute; // Chosen flight route
    private int currentFlightPoint = 0; // Current waypoint index in the route

    private float idleTimer = 0f; // Timer for idle animations
    private float idleInterval = 5f; // Average interval for switching to "eat" animation

    void Start()
    {
        // Set a random interval for the first idle-to-eat transition
        idleInterval = Random.Range(3f, 7f);

        // Choose a random route
        if (flightRoutes.Length > 0)
        {
            Transform routeParent = flightRoutes[Random.Range(0, flightRoutes.Length)];
            currentRoute = new Transform[routeParent.childCount];

            for (int i = 0; i < routeParent.childCount; i++)
            {
                currentRoute[i] = routeParent.GetChild(i); // Store waypoints from the chosen route
            }
        }

        // Play idle sound when bird starts idle
        PlayIdleSound();
    }

    public void UpdateFlightRoutes(Transform[] routes)
    {
        flightRoutes = routes;

        if (flightRoutes.Length > 0)
        {
            Transform routeParent = flightRoutes[Random.Range(0, flightRoutes.Length)];
            currentRoute = new Transform[routeParent.childCount];

            for (int i = 0; i < routeParent.childCount; i++)
            {
                currentRoute[i] = routeParent.GetChild(i); // Store waypoints from the chosen route
            }

        }
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
        if (other.CompareTag("Player") && !isFlying && currentRoute != null)
        {
            isFlying = true;
            animator.SetTrigger("Fly"); // Start flying animation
            PlayFlySound();
            StartFlying();
        }
    }

    void StartFlying()
    {
        if (currentFlightPoint < currentRoute.Length)
        {
            Vector3 target = currentRoute[currentFlightPoint].position;
            StartCoroutine(FlyToPoint(target));
        }
    }

    System.Collections.IEnumerator FlyToPoint(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);

            // Rotate smoothly toward the target direction
            Vector3 direction = (target - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            yield return null;
        }

        currentFlightPoint++;

        if (currentFlightPoint < currentRoute.Length)
        {
            StartFlying(); // Move to the next waypoint
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
