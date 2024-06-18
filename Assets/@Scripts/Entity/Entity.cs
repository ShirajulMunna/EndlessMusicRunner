using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entity : MonoBehaviour, IEntity_Hp, IEntity_Spin
{
    #region IEntytiy_Hp

    [SerializeField] private int maxHp;
    public int MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }
    [SerializeField] private int curHp;
    public int CurHp
    {
        get => curHp;
        set => curHp = value;
    }

    public virtual void SetHp(int value)
    {
        CurHp += value;

        SetAddHp(value);
        SetMinusHp(value);

        if (!CheckDie())
        {
            return;
        }

        SetDie();
    }
    //HP획득
    public void SetAddHp(int value)
    {
        if (value <= 0)
        {
            return;
        }
    }

    //Hp 감소
    public virtual void SetMinusHp(int value)
    {
        if (value > 0)
        {
            return;
        }
    }

    //사망 체크
    public bool CheckDie()
    {
        return CurHp <= 0;
    }

    //죽었을때 처리
    public virtual void SetDie()
    {
        var check = CheckDie();
        if (!check)
        {
            return;
        }
    }
    #endregion


    #region IEntity_Spin
    [SerializeField] SkeletonAnimation _skeletonAnimation;
    public SkeletonAnimation skeletonAnimation
    {
        get => _skeletonAnimation;
        set => _skeletonAnimation = value;
    }

    //애니메이션 처리
    public virtual void SetAni((string, bool) data)
    {
        skeletonAnimation.SetAni_Player(data.Item1, data.Item2);
    }

    public List<string> GetSpineAnimationNames(SkeletonAnimation skeletonAnimation)
    {
        List<string> animationNames = new List<string>();

        if (skeletonAnimation != null && skeletonAnimation.Skeleton != null)
        {
            var animationStateData = skeletonAnimation.SkeletonDataAsset.GetAnimationStateData();
            foreach (var animation in animationStateData.SkeletonData.Animations)
            {
                animationNames.Add(animation.Name);
            }
        }

        return animationNames;
    }
    #endregion
}
