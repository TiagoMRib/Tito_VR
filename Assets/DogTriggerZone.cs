using UnityEngine;

public class DogTriggerZone : MonoBehaviour
{
    public NPCDog npcDog; // Reference to the dog script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && npcDog != null)
        {
            npcDog.TriggerDog();
        }
    }
}
