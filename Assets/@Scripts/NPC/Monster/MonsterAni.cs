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

    [SerializeField] List<St_Monster_Ani> st_Monster_Anis;

    Dictionary<E_AniKind_Monster, string> D_Ani = new Dictionary<E_AniKind_Monster, string>();

    private void Start()
    {
        _sk = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        SetAniData();
        SetPlayerSkin();
    }

    /// <summary>
    /// 애니메이션 데이터 적용
    /// </summary>
    void SetAniData()
    {
        if (st_Monster_Anis == null || st_Monster_Anis.Count <= 0)
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

    public void SetAni(E_AniKind_Monster state, bool loop)
    {
        var ani = GetAniString(state);
        SetAni(ani, false, null);
    }
}


public struct St_Monster_Ani
{
    public E_AniKind_Monster e_AniKind_Monster;
    public string Ani;
}