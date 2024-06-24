using UnityEngine;
using UnityEngine.SceneManagement;

public class SecenManager : MonoBehaviour
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}