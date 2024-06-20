using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;


public class Entity : MonoBehaviour, IEntity_Hp, IEntity_Spin, IEntity_State
{
    #region IEntytity_State
    public E_Entity_State e_Entity_State { get; set; }

    public virtual E_Entity_State GetState()
    {
        return e_Entity_State;
    }

    public virtual void SetState(E_Entity_State state)
    {
        switch (state)
        {
            case E_Entity_State.None:
                break;
            case E_Entity_State.Hit:
                SetHit();
                if (!CheckDie())
                {
                    break;
                }
                SetState(E_Entity_State.Die);
                return;
            case E_Entity_State.Running:
                SetRunning();
                break;
            case E_Entity_State.Die:
                SetDie();
                break;
            case E_Entity_State.Clear:
                SetClear();
                break;
            case E_Entity_State.Fly:
                SetFly();
                break;
            case E_Entity_State.idle:
                SetIdle();
                break;
        }

        e_Entity_State = state;
    }

    public virtual void SetRunning()
    {

    }

    public virtual void SetHit()
    {

    }

    public virtual void SetFly()
    {

    }
    public virtual void SetIdle()
    {

    }

    public virtual void UpdateState()
    {
        UpdateAll();
        switch (e_Entity_State)
        {
            case E_Entity_State.None:
                break;
            case E_Entity_State.Hit:
            case E_Entity_State.Running:
            case E_Entity_State.Fly:
                UpdateIdle();
                break;
            case E_Entity_State.Die:
                UpdateDie();
                break;
            case E_Entity_State.Clear:
                UpdateClear();
                break;
        }
    }

    public virtual void UpdateAll()
    {

    }

    public virtual void UpdateDie()
    {

    }
    public virtual void UpdateClear()
    {

    }
    public virtual void UpdateIdle()
    {

    }
    #endregion

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
        SetState(E_Entity_State.Hit);
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

    public virtual void SetClear()
    {

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
    public virtual void SetAni((string,bool)data ,string name)
    {
        skeletonAnimation.SetAni_Player(data.Item1, data.Item2,name);
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
