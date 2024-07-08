using UnityEngine;

public class GameLog : MonoBehaviour
{
    public static void Log(string log)
    {
#if UNITY_EDITOR
        Debug.Log(log);
#endif
    }
}