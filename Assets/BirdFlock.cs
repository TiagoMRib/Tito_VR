using UnityEngine;

public class BirdFlock : MonoBehaviour
{
    public GameObject birdPrefab; // Bird prefab to instantiate
    public int flockSize = 12; // Total number of birds in the flock
    public Transform[] flightRoutes; // Shared flight routes for the flock
    public DayNightCycle dayNightCycle; // Reference to the DayNightCycle script

    private int currentBirdCount = 0; // Tracks the current number of birds in the flock

    private void Start()
    {
        
    }

    private void Update()
    {
        // Trigger replenishing birds at the start of a new day
        if (IsMorning())
        {
            ReplenishFlock();
        }
    }

    private bool IsMorning()
    {
        // Check if it's morning based on the day-night cycle's normalized time
        if (dayNightCycle != null)
        {
            float normalizedTime = dayNightCycle.currentTime / dayNightCycle.timePerCycle;
            return normalizedTime >= 0.0f && normalizedTime <= 0.25f; // Example: morning is 0-25% of the cycle
        }
        return false;
    }

    private void ReplenishFlock()
    {
        // Replenish birds until the flock size is met
        while (currentBirdCount < flockSize)
        {
            SpawnBird();
        }
    }

    private void SpawnBird()
    {
        // Instantiate a bird and set it as a child of the flock
        GameObject bird = Instantiate(birdPrefab, transform);
        Vector2 randomCircle = Random.insideUnitCircle * 10f;
        bird.transform.position = new Vector3(transform.position.x + randomCircle.x, 0f, transform.position.z + randomCircle.y); // Spawn near the flock position
        bird.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0); // Random orientation

        // Assign a random flight route to the bird
        BirdController birdController = bird.GetComponent<BirdController>();
        if (birdController != null && flightRoutes.Length > 0)
        {
            birdController.UpdateFlightRoutes(flightRoutes); 
        }

        currentBirdCount++;
    }

    public void BirdScared()
    {
        // Decrement the bird count when a bird is scared away
        currentBirdCount = Mathf.Max(0, currentBirdCount - 1);
    }
}
