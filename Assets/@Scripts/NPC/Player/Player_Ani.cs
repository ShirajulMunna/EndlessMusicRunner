using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class Player_Ani : MonoBehaviour, IAni
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

    Dictionary<E_AniKind_Player, string> D_AniName = new Dictionary<E_AniKind_Player, string>()
    {
        {E_AniKind_Player.Idle, "idle"},
        {E_AniKind_Player.Die, "retire"},
        {E_AniKind_Player.Hit, "Hit"},

        {E_AniKind_Player.Running, "running"},
        {E_AniKind_Player.Down, "running"},
        {E_AniKind_Player.DownAttack_0, "running_Attack1"},
        {E_AniKind_Player.DownAttack_1, "running_Attack2"},
        {E_AniKind_Player.DownAttack_2, "running_Attack3"},
        {E_AniKind_Player.DownHoldAttack, "running_Attack_long_note"},

        {E_AniKind_Player.Fly, "fly"},
        {E_AniKind_Player.FlyAttack_0, "fly_Attack1"},
        {E_AniKind_Player.FlyAttack_1, "fly_Attack2"},
        {E_AniKind_Player.FlyHoldAttack, "fly_Attack_long_note"},

        {E_AniKind_Player.Twin, "Twin_Attack"},

        {E_AniKind_Player.Clear_S, "Clear_3"},
        {E_AniKind_Player.Clear_A, "Clear_2"},
        {E_AniKind_Player.Clear_F, "Clear_1"},
    };

    public NPC nPC;

    int AttackCount = 0;

    private void Start()
    {
        SetPlayerSkin();
        nPC = GetComponent<NPC>();
    }

    /// <summary>
    /// 애니메이션 변경
    /// </summary>
    public void SetAni(string str, bool loop, string idle)
    {
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
    /// 애니메이션
    /// </summary>
    public string GetAniString(E_AniKind_Player kind)
    {
        kind = SetAniIdle(kind);
        return D_AniName[kind];
    }


    /// <summary>
    /// 보스 등장시 가만히 서 있도록
    /// </summary>
    E_AniKind_Player SetAniIdle(E_AniKind_Player kind)
    {
        if (kind == E_AniKind_Player.Idle || kind == E_AniKind_Player.Running || kind == E_AniKind_Player.Down)
        {
            var point = nPC.nPC_Move.GetMoveData();

            if (point == E_MoveData.Higt_Higt || point == E_MoveData.Low_Higt)
            {
                return E_AniKind_Player.Fly;
            }

            var checkboss = Bosst.instance.gameObject.activeSelf;
            if (!checkboss)
            {
                kind = E_AniKind_Player.Running;
            }
            else
            {
                kind = E_AniKind_Player.Idle;
            }
        }
        return kind;
    }

    public void SetAttackAni(E_MonsterType types, E_MoveData movepoint)
    {
        if (types == E_MonsterType.Hold)
        {
            SetHold(movepoint);
            return;
        }

        switch (movepoint)
        {
            case E_MoveData.Low_Higt:
            case E_MoveData.Higt_Higt:
                SetFlyAttackAni();
                break;
                
            case E_MoveData.Higt_Middle:
            case E_MoveData.Higt_Low:
            case E_MoveData.Low_Middle:
            case E_MoveData.Low_Low:
                SetDownAttackAni();
                break;

            case E_MoveData.Higt_Twin:
            case E_MoveData.Low_Twin:
                SetTiwn();
                break;

        }
        GameLog.Log($"타입:{types}");
        AttackCount++;
    }

    /// <summary>
    /// 아래 공격
    /// </summary>
    void SetDownAttackAni()
    {
        if (AttackCount > 2)
        {
            AttackCount = 0;
        }
        var anistr = "";
        switch (AttackCount)
        {
            case 1:
                anistr = GetAniString(E_AniKind_Player.DownAttack_1);
                break;
            case 2:
                anistr = GetAniString(E_AniKind_Player.DownAttack_2);
                break;
            default:
                anistr = GetAniString(E_AniKind_Player.DownAttack_0);
                break;
        }
        var idle = GetAniString(E_AniKind_Player.Idle);
        SetAni(anistr, false, idle);
    }

    /// <summary>
    /// Up 공격
    /// </summary>
    void SetFlyAttackAni()
    {
        if (AttackCount > 1)
        {
            AttackCount = 0;
        }
        var anistr = "";
        switch (AttackCount)
        {
            case 1:
                anistr = GetAniString(E_AniKind_Player.FlyAttack_1);
                break;
            default:
                anistr = GetAniString(E_AniKind_Player.FlyAttack_0);
                break;
        }
        var idle = GetAniString(E_AniKind_Player.Fly);
        SetAni(anistr, false, idle);
    }

    /// <summary>
    /// 트윈 공격
    /// </summary>
    void SetTiwn()
    {
        var anistr = GetAniString(E_AniKind_Player.Twin);
        var idle = GetAniString(E_AniKind_Player.Idle);
        SetAni(anistr, false, idle);
    }

    /// <summary>
    /// 홀드
    /// </summary>
    void SetHold(E_MoveData movepoint)
    {
        var checkup = movepoint == E_MoveData.Higt_Higt || movepoint == E_MoveData.Low_Higt;
        var anistr = checkup ? GetAniString(E_AniKind_Player.FlyHoldAttack) : GetAniString(E_AniKind_Player.DownHoldAttack);
        var idle = GetAniString(E_AniKind_Player.Idle);
        SetAni(anistr, false, idle);
    }
}

