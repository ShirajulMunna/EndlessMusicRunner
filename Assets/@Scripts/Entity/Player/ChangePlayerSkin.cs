using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerSkin : MonoBehaviour
{
    public List<string> skin_Names = new List<string>();
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
