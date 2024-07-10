using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class MonsterAni : MonoBehaviour, IAni, IMonsterAni
{
    SkeletonAnimation _sk;
    public SkeletonAnimation sk
    {
        get => _sk;
        set => _sk = value;
    }
    public List<string> skin_Names { get; set; } = new()
    {
        "skin4","skin6","skin0","skin3","skin1","skin2","skin5","skin7" //그래픽 변경
    };

    public int AttackCount { get; set; }
    public int HitCount { get; set; }

    Dictionary<E_AniKind_Monster, string> D_Ani = new Dictionary<E_AniKind_Monster, string>()
    {
        { E_AniKind_Monster.idle,"idle"},
    };

    private void Start()
    {
        _sk = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        SetPlayerSkin();
    }

    /// <summary>
    /// 애니메이션 변경
    /// </summary>
    public void SetAni(string str, bool loop, string idle, bool dir = false)
    {
        if (string.IsNullOrEmpty(str))
        {
            return;
        }

        _sk.SetAni(str, loop, idle);
    }

    /// <summary>
    /// 스킨 변경
    /// </summary>
    public void SetPlayerSkin()
    {
        sk.Skeleton.SetSkin(skin_Names[(int)UI_Lobby.playerSkinType]);
        sk.Skeleton.SetSlotsToSetupPose();
        sk.AnimationState.Apply(sk.Skeleton);
    }

    /// <summary>
    /// 애니메이션 이름
    /// </summary>
    /// <returns></returns>
    public string GetAniString(E_AniKind_Monster e_AniKind_Monster)
    {
        D_Ani.TryGetValue(e_AniKind_Monster, out var data);

        return data;
    }

    public void SetAni(E_AniKind_Monster state, bool loop, bool dir = false)
    {
        var ani = GetAniString(state);
        SetAni(ani, false, null, dir);
    }

    public void SetAni(string aniname, bool loop)
    {
        SetAni(aniname, loop, GetAniString(E_AniKind_Monster.idle));
    }
}


public struct St_Monster_Ani
{
    public E_AniKind_Monster e_AniKind_Monster;
    public string Ani;
}