using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] TMP_Dropdown skinDropDown;
    [SerializeField] TextMeshProUGUI T_Type;
    public static bool Type;
    public static PlayerSkinType playerSkinType = PlayerSkinType.Skin0;


    public SkeletonGraphic playerUiGraphic;
    private List<string> skin_Names = new()
    {
        "skin0","skin1","skin2","skin3","skin4","skin5","skin6","skin7"
    };
    private void Start()
    { 
        if (T_Type == null)
            return;
        T_Type.text = Type ? "Red" : "BLue";
    }

    public void Btn_A_And_B()
    {
        var text = T_Type.text;
        if (text == "BLue")
        {
            text = "Red";
            Type = true;
        }
        else
        {
            text = "BLue";
            Type = false;
        }
        T_Type.text = text;
    }
    public void SetSkin()
    {
        playerSkinType = (PlayerSkinType)skinDropDown.value;
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
        SecenManager.LoadScene("MainGameScene");
    }

}


public enum PlayerSkinType
{
    Skin0, Skin1, Skin2, Skin3,
    Skin4, Skin5, Skin6, skin7, Count
}
