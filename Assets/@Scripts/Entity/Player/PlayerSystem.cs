using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Spine.Unity;
using UnityEngine;

public class PlayerSystem : Entity
{
    public static PlayerSystem playerSystem;

    [SerializeField] List<IPlayer_Particle> L_Particle = new List<IPlayer_Particle>();

    //공격 클래스
    [HideInInspector] public IPlayer_Attack M_Attack;
    //이동 클래스
    [HideInInspector] public IPlayer_Move M_Move;

    //애니메이션 리스트
    List<string> L_AniStr = new List<string>()
    {
        "Running",
        "fly",
        "Kick",
        "tail attack",
        "fire attack",
        "tail attack2",
        "retire",
    };

    private void Awake()
    {
        playerSystem = this;
        M_Attack = new IPlayer_Attack();
        M_Move = new IPlayer_Move();
    }

    private void Start()
    {
        var result = GetAniName(E_AniType.Running);
        SetAni(result);
        UI_Play.Instance.ActivatPanel(true);
        SetParticle(E_PlayerSkill.Running, 0);
    }

    private void Update()
    {
        var point = M_Attack.Attack();
        M_Move.SetMove(point);
        SetParticle_Active();
    }

    //애니메이션 이름 가져오기
    public (string, bool) GetAniName(E_AniType state)
    {
        return (L_AniStr[(int)state], state == E_AniType.Running || state == E_AniType.Fly);
    }

    public override void SetHp(int value)
    {
        //hp감소일때 쉴드 체크
        if (value < 0)
        {
            var checkshield = ShieldBuster.CheckShield();
            if (checkshield)
            {
                return;
            }
        }
        base.SetHp(value);
        UI_Play.Instance.SetHp(MaxHp, CurHp);
    }

    public override void SetMinusHp(int value)
    {
        base.SetMinusHp(value);

        //공격 사운드 및 애니메이션 처리
        SetAni(GetAniName(E_AniType.Hit));
    }

    public override void SetDie()
    {
        base.SetDie();

        //사망 후 처리
        AudioManager.instance.StopMusic();
        SpawnManager.instance.isCompltedGame = true;
    }

    //파티클 실행
    public void SetParticle(E_PlayerSkill skilltype, float activetime)
    {
        if (L_Particle.Count <= 0)
        {
            return;
        }

        var data = L_Particle.FindAll(x => x.SkillType == skilltype);

        if (data.Count <= 0)
        {
            return;
        }

        foreach (var item in data)
        {
            item.SetParticle(activetime);
        }

        //Fly, Running체크해서 파티클 끄고 키기
        var checkrunning = data[0].SetIdle();
        if (checkrunning)
        {
            var type = skilltype == E_PlayerSkill.Fly ? E_PlayerSkill.Running : E_PlayerSkill.Fly;
            data = L_Particle.FindAll(x => x.SkillType == type);
            foreach (var item in data)
            {
                item.SetDirectActive(false);
            }
        }
    }

    //파티클 쿨타임 진행
    void SetParticle_Active()
    {
        if (L_Particle.Count <= 0)
        {
            return;
        }
        foreach (var item in L_Particle)
        {
            item.SetActive();
        }
    }
}