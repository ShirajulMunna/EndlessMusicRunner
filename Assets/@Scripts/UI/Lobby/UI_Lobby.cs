using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] TMP_Dropdown skinDropDown;
    [SerializeField] TextMeshProUGUI T_Type;
    [SerializeField] TMP_Dropdown BitDropDown;
    [SerializeField] SkeletonGraphic playerUiGraphic;

    public static bool Type;
    public static PlayerSkinType playerSkinType = PlayerSkinType.Skin0;
    static int ScrollValue;
    public static string Str_BitName;

    private List<string> skin_Names = new()
    {
        "skin4","skin6","skin0","skin3","skin1","skin2","skin5","skin7" //그래픽 변경
    };
    private void Start()
    {
        SetBit();
        if (T_Type == null)
            return;

        Type = true;
        T_Type.text = !Type ? "VsMode" : "RunMode";
        skinDropDown.value = ScrollValue;
        ChangePlayerUiGraphics();
    }

    public void Btn_A_And_B()
    {
        var text = T_Type.text;
        Type = !Type;

        if (!Type)
        {
            text = "VsMode";
        }
        else
        {
            text = "RunMode";
        }
        T_Type.text = text;
    }

    public void SetBit()
    {
        Str_BitName = BitDropDown.options[BitDropDown.value].text;
    }

    public void SetSkin()
    {
        var value = skinDropDown.value;
        // PD님 요청으로 그래픽타입 순서 변경 코드 -> UI에서는 1부터시작으로 -1해야함 
        ScrollValue = skinDropDown.value;
        playerSkinType = (PlayerSkinType)value;
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
        var mode = !Type ? "NotMoveBackGroundScene" : "MainGameScene";
        SecenManager.LoadScene(mode);
    }

}


public enum PlayerSkinType
{
    Skin0, Skin1, Skin2, Skin3,
    Skin4, Skin5, Skin6, skin7, Count
}
