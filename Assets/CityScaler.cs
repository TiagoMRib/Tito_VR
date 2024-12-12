using UnityEngine;

public class CityScaler : MonoBehaviour
{
    [Header("Scaling Parameters")]
    [Range(1f, 10f)]
    public float scaleFactor = 2f; // A scaling multiplier to control how much larger buildings get.

    [Header("Player Reference")]
    public Transform player; // Reference to the player to compare sizes.

    void Start()
    {
        ScaleBuildings();
    }

    void ScaleBuildings()
    {
        // Iterate through each child object (the building prefabs).
        foreach (Transform building in transform)
        {
            // Iterate through each child of the building, which is the actual model.
            foreach (Transform model in building)
            {
                // Get the current height of the model (in the Y axis).
                float originalHeight = model.localScale.z;
                // Calculate the scale factor based on height.
                float scaleMultiplier = Mathf.Lerp(1f, scaleFactor, originalHeight / 10f); // Adjust divisor for tuning.
                
                // Optionally, you can compare against the player's height to avoid over-scaling small objects.
                if (originalHeight < player.localScale.z) 
                {
                    scaleMultiplier = 1f; // Don't scale up very small objects (like the mailbox).
                }

                // Apply the scaling to the Y-axis only (keeping X and Z the same).
                model.localScale = new Vector3(model.localScale.x, model.localScale.y , model.localScale.z * scaleMultiplier);
            }
        }
    }
}
