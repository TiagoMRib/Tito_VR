using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI messageText; // Reference to the Text element in the Canvas

    void Start()
    {
        // Ensure the text is empty at the start
        messageText.text = "";
    }

    // Show a new message
    public void ShowMessage(string message)
    {
        messageText.text = message;
        Debug.Log("The text is now:" + messageText.text);
    }

    // Clear the currently displayed message
    public void ClearMessage()
    {
        messageText.text = "";
    }
}
