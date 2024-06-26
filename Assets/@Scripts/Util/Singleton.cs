using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private static bool applicationIsQuitting = false;

    public static T instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString() + " (Singleton)";
                    }
                }

                return _instance;
            }
        }
    }

    // 씬이동되면서 자동 Destory되기때문에 true로인해서 스폰매니저 객체에 접근이 불가능한 문제가 발생함
    //protected virtual void OnDestroy()
    //{
    //    applicationIsQuitting = true;
    //}
}
