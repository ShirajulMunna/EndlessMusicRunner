using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Lobby : Singleton<UI_Lobby>
{
    [SerializeField] SkeletonGraphic playerUiGraphic;

    public static bool Type;
    public static string Str_BitName;
    public static int BitIdx;
    List<string> _BitName = new List<string>()
    {
        "Ellagator_S1E1",
        "Ellagator_S8E2",
    };

    private void Start()
    {
        SetBit(BitIdx);
        ActionManager.instance.SetAction((E_ActionScene.Lobby, E_ActionList.Start));
    }

    public void SetBit(int idx)
    {
        BitIdx = idx;
        Str_BitName = _BitName[idx];
    }

    public int MaxBit()
    {
        return _BitName.Count - 1;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public async void Btn_Play()
    {
        await AudioManager.instance.GetPlayMusic();
        var mode = "MainGameScene";
        SecenManager.LoadScene(mode);
    }
}