using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Lobby : Singleton<UI_Lobby>
{
    [SerializeField] SkeletonGraphic playerUiGraphic;

    public static bool Type;
    public static PlayerSkinType playerSkinType = PlayerSkinType.Skin0;
    public static string Str_BitName;
    public static int BitIdx;

    private List<string> skin_Names = new()
    {
        "skin0","skin4","skin6","skin3","skin1","skin2","skin5","skin7" //그래픽 변경
    };

    List<string> _BitName = new List<string>()
    {
        "Ellagator_S1E1_MIX_2",
        "2nd"
    };

    private void Start()
    {
        SetBit(BitIdx);
        ChangePlayerUiGraphics();
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

    public void ChangePlayerUiGraphics()
    {
        playerUiGraphic.Skeleton.SetSkin(skin_Names[(int)UI_Lobby.playerSkinType]);
        playerUiGraphic.Skeleton.SetSlotsToSetupPose();
        playerUiGraphic.AnimationState.Apply(playerUiGraphic.Skeleton);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void Btn_Play()
    {
        var mode = "MainGameScene";
        SecenManager.LoadScene(mode);
    }
}

public enum PlayerSkinType
{
    Skin0, Skin1, Skin2, Skin3,
    Skin4, Skin5, Skin6, skin7, Count
}
