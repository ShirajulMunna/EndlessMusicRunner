using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    const string StrName = "UtilSound_0";
    private void Start()
    {
        SetBtnSound();
    }

    void SetBtnSound()
    {
        var but = this.GetComponentsInChildren<Button>();

        foreach (var item in but)
        {
            item.onClick.AddListener(() => AudioManager.instance.PlayEffectSound(StrName));
        }
    }
}