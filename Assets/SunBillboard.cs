using UnityEngine;

public class SunBillboard : MonoBehaviour
{
    public Transform player; // Reference to the player (camera) to always face

    public Transform sunLightSource; // The actual directional light for the sun
    public float visibleDistance = 100f; // Distance at which the sun billboard appears

    void Update()
    {
        if (player == null || sunLightSource == null)
            return;

        // Position the billboard at a fixed distance in front of the player
        Vector3 directionToSun = (sunLightSource.position - player.position).normalized;
        transform.position = player.position + directionToSun * visibleDistance;

        // Make the billboard face the player
        transform.LookAt(player);

        // Optional: Scale the billboard based on the distance to maintain consistent size
        float scale = 10f; // Adjust to fit your scene
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
