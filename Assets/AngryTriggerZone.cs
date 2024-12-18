using UnityEngine;

public class AngryTriggerZone : MonoBehaviour
{
    public NPCShouter npc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && npc != null)
        {
            npc.TriggerShout();
        }
    }
}