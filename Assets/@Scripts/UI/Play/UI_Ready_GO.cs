using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class UI_Ready_GO : MonoBehaviour
{
    const string ready_go = "UI_Ready_GO";
    public static async void Create(float times)
    {
        var result = await ready_go.CreateOBJ<UI_Ready_GO>();
        result.SetUp(times);
    }

    [SerializeField] GameObject[] G_OBJ;

    public void SetUp(float times)
    {
        StartCoroutine(IE_Delay(times));
    }


    IEnumerator IE_Delay(float times)
    {
        G_OBJ[0].SetActive(true);
        var go = (times / 3);
        var ready = times - go;
        yield return new WaitForSeconds(ready);
        G_OBJ[1].SetActive(true);
        G_OBJ[0].SetActive(false);
        yield return new WaitForSeconds(go);
        G_OBJ[1].SetActive(false);
    }
}
