using UnityEngine;
using UnityEngine.AI;

public class CarMovement : MonoBehaviour
{
    public float turnSpeed = 2f;                  // Speed at which the car turns
    public float destinationChangeInterval = 3f;  // Time interval to change the destination
    public float maxRoamDistance = 10f;           // Maximum distance to roam in the scene
    public float curveDetectionThreshold = 30f;   // Angle threshold to detect a curve (in degrees)
    public Transform[] wheels;
    public float wheelRadius = 0.33f;            // Radius of the wheels in meters
 
    private NavMeshAgent navMeshAgent;            // Reference to the NavMeshAgent
    private Vector3 currentDestination;           // Current destination for the car to roam to
    private float timeSinceLastChange;            // Timer for changing the destination

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        navMeshAgent.angularSpeed = turnSpeed;       // Set turn speed for the agent
        SetNewDestination();                         // Initial destination
    }


    void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        // If the car has reached the destination or enough time has passed, set a new destination
        if (timeSinceLastChange >= destinationChangeInterval || Vector3.Distance(transform.position, currentDestination) < navMeshAgent.stoppingDistance)
        {
            SetNewDestination();
            timeSinceLastChange = 0f; // Reset the timer
        }

        // Detect if the car is approaching a curve
        DetectCurve();

        // Try to move forward towards the destination and rotate smoothly
        MoveCar();

        RotateWheels();
    }
    void RotateWheels()
    {
        if (wheels.Length != 4) return; // Ensure there are 4 wheels assigned

        // Calculate the distance traveled since the last frame
        float distanceTraveled = navMeshAgent.velocity.magnitude * Time.deltaTime;

        // Calculate the rotation angle based on the distance and wheel radius
        float rotationAngle = (distanceTraveled / (2f * Mathf.PI * wheelRadius)) * 360f;

        // Rotate each wheel
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(Vector3.right, rotationAngle, Space.Self); // Rotate around the local X-axis
        }

        // Rotate the front wheels for steering
        Vector3 targetDirection = currentDestination - transform.position;
        Vector3 moveDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up);

         /*
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float steerAngle = Quaternion.Angle(transform.rotation, targetRotation);

            // Apply steering rotation to the front wheels
            wheels[0].localRotation = Quaternion.Euler(0, Mathf.Clamp(steerAngle, -30f, 30f), 0); // Front-left
            wheels[1].localRotation = Quaternion.Euler(0, Mathf.Clamp(steerAngle, -30f, 30f), 0); // Front-right
        }
        */
    }

    void MoveCar()
    {
        Vector3 targetDirection = currentDestination - transform.position;
        Vector3 moveDirection = Vector3.ProjectOnPlane(targetDirection, Vector3.up); // Keep it on the horizontal plane

        if (moveDirection.magnitude > 0.1f)
        {
            // Rotate the car to face the destination
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Move the car forward using the NavMeshAgent
            navMeshAgent.Move(transform.forward * navMeshAgent.speed * Time.deltaTime);
        }
    }

    void SetNewDestination()
    {
        // Generate a new random destination within a certain range from the current position
        Vector3 randomDirection = Random.insideUnitSphere * maxRoamDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, maxRoamDistance, NavMesh.AllAreas))
        {
            currentDestination = hit.position;
            navMeshAgent.SetDestination(currentDestination); // Set the new destination for the agent
        }
    }

    void DetectCurve()
    {
        if (navMeshAgent.path.corners.Length < 2) return;  // If no waypoints are available, exit

        // Get the current direction of the car (forward)
        Vector3 currentDirection = transform.forward;

        // Get the direction to the next corner in the NavMesh path
        Vector3 nextWaypoint = navMeshAgent.path.corners[1] - transform.position;
        nextWaypoint = Vector3.ProjectOnPlane(nextWaypoint, Vector3.up); // Keep it on the horizontal plane

        // Calculate the angle between the current direction and the direction to the next waypoint
        float angle = Vector3.Angle(currentDirection, nextWaypoint);

        // If the angle exceeds the threshold, it's likely a curve
        if (angle > curveDetectionThreshold)
        {
            // The car is approaching a curve
            Debug.Log("Approaching a curve!");

            // You can add any additional logic here to handle the curve (e.g., adjusting speed or steering)
        }
    }
}
