using System.Collections.Generic;
using System.Linq.Expressions;
using Spine.Unity;
using UnityEngine;

public class MonsterAni_Special : MonoBehaviour, IAni, IMonsterAni
{
    [SerializeField] SkeletonAnimation _sk;
    public SkeletonAnimation sk
    {
        get => _sk;
        set => _sk = value;
    }
    public List<string> skin_Names { get; set; } = new()
    {
        "skin4","skin6","skin0","skin3","skin1","skin2","skin5","skin7" //그래픽 변경
    };
    public int HitCount { get; set; }

    [SerializeField] List<St_Monster_Ani> st_Monster_Anis;

    Dictionary<E_AniKind_Monster, string> D_Ani = new Dictionary<E_AniKind_Monster, string>();

    private void Start()
    {
        SetAniData();
        SetPlayerSkin();
    }

    /// <summary>
    /// 애니메이션 데이터 적용
    /// </summary>
    void SetAniData()
    {
        if (st_Monster_Anis == null)
        {
            return;
        }

        foreach (var item in st_Monster_Anis)
        {
            D_Ani[item.e_AniKind_Monster] = item.Ani;
        }
    }

    /// <summary>
    /// 애니메이션 변경
    /// </summary>
    public void SetAni(string str, bool loop, string idle)
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

    E_AniKind_Monster SetHit(E_AniKind_Monster state)
    {
        if (state != E_AniKind_Monster.Hit_0)
        {
            return state;
        }

        var types = E_AniKind_Monster.Hit_0;
        switch (HitCount)
        {
            case 1:
                types = E_AniKind_Monster.Hit_1;
                break;
            case 2:
                types = E_AniKind_Monster.Hit_2;
                break;
        }

        return types;
    }

    public void SetAni(E_AniKind_Monster state, bool loop)
    {
        state = SetHit(state);
        var ani = GetAniString(state);
        SetAni(ani, loop, GetAniString(E_AniKind_Monster.idle));
    }

    public void SetAni(string aniname, bool loop)
    {
        SetAni(aniname, loop, GetAniString(E_AniKind_Monster.idle));
    }
}