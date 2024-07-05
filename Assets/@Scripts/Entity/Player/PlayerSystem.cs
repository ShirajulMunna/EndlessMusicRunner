using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : Entity
{
    public static PlayerSystem playerSystem;

    [SerializeField] List<IPlayer_Particle> L_Particle = new List<IPlayer_Particle>();
    public bool isHigt;

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
        "running",
        "fly",
        "running_Attack1",
        "running_Attack2",
        "running_Attack3",
        "fly_Attack1",
        "running_Attack_long_note",
        "Hit",
        "fly_Attack2",
        "retire",
        "fly_Attack_long_note",
        "Clear_3",// 클리어 애니메이션추가
        "idle",
        "Clear_2",// 클리어 애니메이션추가
        "Clear_1",// 클리어 애니메이션추가
    };

    public bool isStopPlayer = false;

    private void Awake()
    {
        playerSystem = this;
        M_Attack = GetComponent<IPlayer_Attack>();
        M_Move = GetComponent<IPlayer_Move>();
        M_State = GetComponent<IPlayer_KeyPoint>();
        M_State.isHigt = isHigt;
    }

    private void Start()
    {
        if (!isHigt)
        {
            UI_Play.Instance.ActivatPanel(true);
        }
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
    public void SetisStop(bool state)
    {
        isStopPlayer = state;
    }

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

    [SerializeField] float dleays;
    bool Checkss;
    public override void UpdateClear()
    {
        base.UpdateClear();
        M_Move.IsClearMove();

        dleays -= Time.deltaTime;

        if (dleays > 0)
        {
            return;
        }

        if (!Checkss)
        {
            M_Move.SetClearMoveTime();
            CameraSystem.cameraSystem.SetZoomIn(true);
            Checkss = true;
            var rank = ScoreManager.instance.GetScoreRank();
            switch (rank)
            {
                case ScoreManager.ScoreRank.S:
                    SetAni(GetAniName(E_AniType.Clear_S));
                    break;
                case ScoreManager.ScoreRank.A:
                case ScoreManager.ScoreRank.B:
                    SetAni(GetAniName(E_AniType.Clear_A));
                    break;
                default:
                    SetAni(GetAniName(E_AniType.Clear_C));
                    break;
            }
        }
    }

    public override void SetRunning()
    {
        base.SetRunning();
        var result = ("", false);
        if (isStopPlayer)
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

        var checkhp = GameManager.instance.player_1.CurHp > GameManager.instance.player.CurHp;

        if (checkhp)
        {
            GameManager.instance.player_1.CurHp = GameManager.instance.player.CurHp;
        }
        else
        {
            GameManager.instance.player.CurHp = GameManager.instance.player_1.CurHp;
        }

        UI_Play.Instance.SetHp(MaxHp, CurHp);
        //공격 사운드 및 애니메이션 처리

        SetAni(GetAniName(E_AniType.Hit), GetIdle());

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
        SetAni(GetAniName(E_AniType.Running));
    }

    //게임 종료 처리들
    void EndGame()
    {
        if (!isHigt)
        {
            M_Move.DirectMove(E_MovePoint.Down);
        }
        else
        {
            M_Move.DirectMove(E_MovePoint.Down, 1.1f);
        }

        OffAllL_Particle();
        AudioManager.instance.StopMusic();
        SpawnManager.instance.SetState(E_GameState.End);
    }
    #endregion

    #region 애니메이션

    public string GetIdle()
    {
        var idle_Type = M_Move.GetPoint() == E_MovePoint.Up ? "fly" : "running";

        if (!UI_Lobby.Type || isStopPlayer)
        {
            idle_Type = M_Move.GetPoint() == E_MovePoint.Up ? "fly" : "idle";
        }

        return idle_Type;
    }

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
            || state == E_AniType.Die || state == E_AniType.idle || state == E_AniType.Hold_Attack || state == E_AniType.Hold_Fly_Attack);
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

        foreach (var item in L_Particle)
        {
            item.SetChange(skilltype);
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
}