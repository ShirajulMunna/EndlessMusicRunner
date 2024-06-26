using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerSystem : Entity
{
    public static PlayerSystem playerSystem;

    [SerializeField] List<IPlayer_Particle> L_Particle = new List<IPlayer_Particle>();

    //공격 클래스
    [HideInInspector] public IPlayer_Attack M_Attack;
    //이동 클래스
    [HideInInspector] public IPlayer_Move M_Move;
    //상태별 처리 클래스
    [HideInInspector] public IPlayer_KeyPoint M_State;

    //홀딩 딜레이
    float HoldDelay;
    const float MaxHoldDelay = 0.3f;

    //현재 공격 횟수(애니메이션용)
    int AttackCount = 0;
    const int MaxAttackCount = 3;

    //애니메이션 변경 딜레이
    float noneChange;
    const float MaxnoneChange = 0.2f;

    //애니메이션 리스트
    List<string> L_AniStr = new List<string>()
    {
        "Running",
        "fly",
        "Kick",
        "tail attack",
        "fist attack",
        "fire attack",
        "biting attack",
        "Hit",
        "fly_attack",
        "retire",
        "fly_biting",
        "Clear_2",// 클리어 애니메이션추가
        "idle",
    };

    public bool isStopPlayer = false;

    private void Awake()
    {
        playerSystem = this;
        M_Attack = new IPlayer_Attack();
        M_Move = new IPlayer_Move();
        M_State = new IPlayer_KeyPoint();
    }

    private void Start()
    {
        UI_Play.Instance.ActivatPanel(true);
        SetState(E_Entity_State.Running);
        SpawnManager.instance.Ac_EndGame += () =>
        {
            SetState(E_Entity_State.Clear);
        };
    }

    private void Update()
    {
        UpdateState();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnManager.instance.SetState(E_GameState.End);
        }
    }


    #region 상태처리
    public override void SetIdle()
    {
        base.SetIdle();
        var result = GetAniName(E_AniType.idle);
        SetAni(result);
    }
    public override void SetFly()
    {
        base.SetFly();
        var result = GetAniName(E_AniType.Fly);
        SetAni(result, result.Item1);
        SetParticle(E_PlayerSkill.Fly, 0);
    }

    public override void UpdateClear()
    {
        base.UpdateClear();
        M_Move.IsClearMove();
    }

    public override void SetRunning()
    {
        base.SetRunning();
        var result = ("", false);
        if (SpawnManager.instance != null && SpawnStage.instance.GetStageInfo() >= 1000 || isStopPlayer)
        {
            result = GetAniName(E_AniType.idle);
            SetAni(result, result.Item1);
        }
        else
        {
            result = GetAniName(E_AniType.Running);
            SetParticle(E_PlayerSkill.Running, 0);
            SetAni(result);
        }
    }

    public override void SetHit()
    {
        base.SetHit();
        UI_Play.Instance.SetHp(MaxHp, CurHp);
        //공격 사운드 및 애니메이션 처리
        SetAni(GetAniName(E_AniType.Hit));

        StartCoroutine(DamageEffect());
    }

    public override void UpdateIdle()
    {
        base.UpdateIdle();
        var point = M_State.SetPoint();
        M_Move.SetMove(point);
        var attackstate = M_Attack.Attack(point);
        SetAttackAni(point, attackstate);
    }

    public override void UpdateAll()
    {
        base.UpdateAll();
        SetParticle_Active();
        HoldDelay -= Time.deltaTime;
        noneChange -= Time.deltaTime;
    }
    #endregion

    #region HP처리
    bool CheckShield(int value)
    {
        //hp감소일때 쉴드 체크
        if (value < 0)
        {
            var checkshield = ShieldBuster.CheckShield();
            if (checkshield)
            {
                return true;
            }
        }
        return false;
    }

    public override void SetHp(int value)
    {
        var checkshield = CheckShield(value);
        if (checkshield)
        {
            return;
        }
        base.SetHp(value);
    }

    #endregion

    #region 사망 처리

    //사망 처리 
    public override void SetDie()
    {
        base.SetDie();
        EndGame();
        SetAni(GetAniName(E_AniType.Die));
        SpawnManager.instance.Ac_EndGame = null;
    }

    //클리어 처리
    public override void SetClear()
    {
        base.SetClear();
        EndGame();
        SetAni(GetAniName(E_AniType.Clear));
    }

    //게임 종료 처리들
    void EndGame()
    {
        M_Move.DirectMove(E_MovePoint.Down);
        OffAllL_Particle();
        AudioManager.instance.StopMusic();
        SpawnManager.instance.SetState(E_GameState.End);
    }
    #endregion

    #region 애니메이션

    //애니메이션 셋팅
    void SetAttackAni(E_MovePoint keypoint, E_AttackState e_AttackState)
    {
        if (e_AttackState != E_AttackState.None || keypoint == E_MovePoint.Down)
        {
            noneChange = MaxnoneChange;
            SetAttackCount();
        }
        switch (e_AttackState)
        {
            case E_AttackState.Attack:
                OffAllL_Particle();
                var state = keypoint == E_MovePoint.Down ? GetAttackAniState(E_AniType.Kick, E_AniType.Tail_Attack, E_AniType.Fist_attack) : GetAttackAniState(E_AniType.Fly_Attack, E_AniType.Fire_Attack);
                SetAni(GetAniName(state));
                return;
            case E_AttackState.Hold:
                if (HoldDelay > 0)
                {
                    return;
                }
                HoldDelay = MaxHoldDelay;
                state = keypoint == E_MovePoint.Up ? E_AniType.Hold_Fly_Attack : E_AniType.Hold_Attack;
                SetAni(GetAniName(state));
                return;
            case E_AttackState.Twin_Attack:
                SetAni(("Twin_Attack", false));
                return;
        }
        switch (keypoint)
        {
            case E_MovePoint.Down:

                SetState(E_Entity_State.Running);
                var state = GetAttackAniState(E_AniType.Kick, E_AniType.Tail_Attack, E_AniType.Fist_attack);
                SetAni(GetAniName(state));
                break;
            case E_MovePoint.Up:
                SetState(E_Entity_State.Fly);
                break;
        }
    }

    E_AniType GetAttackAniState(E_AniType zero, E_AniType one)
    {
        var state = AttackCount == 0 ? zero : one;
        return state;
    }
    //하단 공격 모션 추가로 함수 오버로딩
    E_AniType GetAttackAniState(E_AniType zero, E_AniType one, E_AniType two)
    {
        if (AttackCount == 1) return one;
        if (AttackCount == 2) return two;
        return zero;
    }

    //공격 횟수 수정
    void SetAttackCount()
    {
        AttackCount++;
        if (AttackCount >= MaxAttackCount)
        {
            AttackCount = 0;
        }
    }
    //애니메이션 이름 가져오기
    public (string, bool) GetAniName(E_AniType state)
    {
        return (L_AniStr[(int)state], state == E_AniType.Running || state == E_AniType.Fly
            || state == E_AniType.Die || state == E_AniType.idle);
    }
    #endregion

    #region 파티클

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

    //클리어시 모든 파티클 강제 종료
    public void OffAllL_Particle()
    {
        for (int i = 0; i < L_Particle.Count; ++i)
        {
            L_Particle[i].SetDirectActive(false);
        }
    }

    #endregion

    #region 캐릭터 스파인 투명도 변경
    private float playerMaterialAlpha = 1f;
    private IEnumerator DamageEffect()
    {
        while (true)
        {
            playerMaterialAlpha = 0.5f;
            yield return StartCoroutine(LerpSpinAlpha());

            playerMaterialAlpha = 1f;
            yield return StartCoroutine(LerpSpinAlpha());

            yield break;
        }
    }
    private IEnumerator LerpSpinAlpha()
    {
        float startAlpha = playerMaterialAlpha;
        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            if (skeletonAnimation != null)
            {
                foreach (var slot in skeletonAnimation.Skeleton.Slots)
                {
                    var color = slot.GetColor();
                    color.a = Mathf.Lerp(startAlpha, playerMaterialAlpha, elapsedTime / duration);
                    slot.SetColor(color);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion
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