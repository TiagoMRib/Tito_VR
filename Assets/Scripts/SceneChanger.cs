using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class SceneChanger : MonoBehaviour
{


    public void ChangeScene(string sceneName)
    {
        Debug.Log("Called");
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }

}