using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Spine_GameResult : MonoBehaviour
{
    SkeletonAnimation sk;
  

    public GameResultType type;
    public static async void Create(Vector3 cratepos,GameResultType type    )
    {
        string name = "GameResult_0";
        var result = await name.CreateOBJ<Spine_GameResult>(null,cratepos);
        result.type = type;
    }
    public void Start()
    {
        sk = GetComponent<SkeletonAnimation>();

        sk.skeleton.SetToSetupPose();
        sk.Skeleton.SetSkin(type.ToString());
        sk.Skeleton.SetSlotsToSetupPose();
        sk.AnimationState.Apply(sk.Skeleton);

    }
}
public enum GameResultType
{
    Clear, Failed, Full_combo
}