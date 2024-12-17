using UnityEngine;

public class AdopterTriggerZone : MonoBehaviour
{
    public NPCAdopter npc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && npc != null)
        {
            npc.TriggerAdopter();
        }
    }
}
