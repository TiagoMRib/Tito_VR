using UnityEngine;

public class InfoTrigger : MonoBehaviour
{
    public string infoMessage = "Hey there, bud"; // The message to display

    public InfoDisplay infoDisplay; // Reference to InfoDisplay for showing messages

/*
    private void Start()
    {
        infoDisplay = FindObjectOfType<InfoDisplay>();
        if (infoDisplay == null)
        {
            Debug.LogError("InfoDisplay script is missing from the scene.");
        }
    } */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Display the task message
            infoDisplay.ShowMessage(infoMessage);
        }
    }
}
