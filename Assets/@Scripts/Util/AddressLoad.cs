using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AddressLoad
{
    static Dictionary<string, UnityEngine.Object> D_Resources = new Dictionary<string, UnityEngine.Object>();

    /// <summary>
    /// 생성함수
    /// </summary>
    public static async Task<T> CreateOBJ<T>(this string key, Transform parent = null, Vector3 pos = default, Quaternion rot = default) where T : UnityEngine.Object
    {
        GameObject result = await LoadAsync<GameObject>(key);
        if (result == null)
        {
            Debug.LogError($"Null value loaded for key: {key}");
            return null;
        }

        GameObject go = GameObject.Instantiate(result, pos, rot, parent);
        if (go == null)
        {
            Debug.LogError($"Instantiation failed for key: {key}");
            return null;
        }

        if (typeof(T) == typeof(GameObject))
        {
            return go as T;
        }

        return go.GetComponent<T>();
    }


    /// <summary>
    /// 로드 함수
    /// </summary>
    public static async Task<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        if (D_Resources.TryGetValue(key, out UnityEngine.Object resource) && resource is T)
        {
            return (T)resource;
        }

        T result = await Addressables.LoadAssetAsync<T>(key).Task;
        if (result == null)
        {
            Debug.LogError($"Failed to load asset with key: {key}");
        }
        else
        {
            D_Resources[key] = result;
        }

        return result;
    }
}