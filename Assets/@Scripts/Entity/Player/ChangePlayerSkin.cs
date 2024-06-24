using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerSkin : MonoBehaviour
{
    private List<string> skin_Names = new()
    {
        // "skin0","skin1","skin2","skin3","skin4","skin5","skin6","skin7"
        "skin4","skin6","skin0","skin3","skin1","skin2","skin5","skin7" //그래픽 변경
    };
    SkeletonAnimation skeletonAnimation;

    public void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        SetPlayerSkin();
    }


    public void SetPlayerSkin()
    {
        skeletonAnimation.Skeleton.SetSkin(skin_Names[(int)UI_Lobby.playerSkinType]);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
    }
}
