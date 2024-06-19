using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : Entity
{
    public static PlayerSystem playerSystem;

    [SerializeField] List<IPlayer_Particle> L_Particle = new List<IPlayer_Particle>();

    //공격 클래스
    [HideInInspector] public IPlayer_Attack M_Attack;
    //이동 클래스
    [HideInInspector] public IPlayer_Move M_Move;
    [HideInInspector] public IPlayer_State M_State;

    E_AniType aniType = E_AniType.Running;

    //애니메이션 리스트
    List<string> L_AniStr = new List<string>()
    {
        "Running",
        "fly",
        "Kick",
        "tail attack",
        "fire attack",
        "biting attack",
        "Hit",
        "fly_attack",
        "retire",
        "fly_biting",
        "Clear_2",// 클리어 애니메이션추가
    };


    private void Awake()
    {
        playerSystem = this;
        M_Attack = new IPlayer_Attack();
        M_Move = new IPlayer_Move();
        M_State = new IPlayer_State();
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
        var point = M_State.SetPoint();
        M_Move.SetMove(point);
        M_Attack.Attack(point);
        SetParticle_Active();
    }

    //애니메이션 이름 가져오기
    public (string, bool) GetAniName(E_AniType state)
    {
        return (L_AniStr[(int)state], state == E_AniType.Running || state == E_AniType.Fly || state == E_AniType.Die);
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
        StartCoroutine(DamageEffect());
        base.SetHp(value);

        UI_Play.Instance.SetHp(MaxHp, CurHp);
    }

    private float targetAlpha = 1f;
    private float alphaChangeSpeed = 2f; 
    private IEnumerator DamageEffect()
    {
        while (true)
        {
            targetAlpha = 0.5f;
            yield return StartCoroutine(LerpAlpha());

            // 투명도를 0.5에서 1로 변경
            targetAlpha = 1f;
            yield return StartCoroutine(LerpAlpha());

            yield break;
        }
    }
    private IEnumerator LerpAlpha()
    {
        float startAlpha = targetAlpha;
        float elapsedTime = 0f;
        float duration = 0.75f;

        while (elapsedTime < duration)
        {
            // 실제 투명도 조절
            if (skeletonAnimation != null)
            {
                foreach (var slot in skeletonAnimation.Skeleton.Slots)
                {
                    var color = slot.GetColor();
                    color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                    slot.SetColor(color);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
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
        SetAni(GetAniName(E_AniType.Die));
        AudioManager.instance.StopMusic();
        SpawnManager.instance.SetGameState(E_GameState.End);
        aniType = E_AniType.Die;
    }

    public override void SetAni((string, bool) data)
    {
        if (aniType == E_AniType.Die)
        {
            return;
        }

        base.SetAni(data);
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

    //그림
    private void OnDrawGizmos()
    {
        for (int i = 0; i < 3; i++)
        {
            M_Attack.DrawOverlapBox(i, ScoreManager.E_ScoreState.Perfect, Color.green);

            M_Attack.DrawOverlapBox(i, ScoreManager.E_ScoreState.Early, Color.yellow);

            M_Attack.DrawOverlapBox(i, ScoreManager.E_ScoreState.Late, Color.red);

            M_Attack.DrawOverlapBox(i, ScoreManager.E_ScoreState.Great, Color.blue);
        }
    }
}