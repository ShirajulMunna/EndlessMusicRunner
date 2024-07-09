using Unity.VisualScripting;
using UnityEngine;

public class Player : NPC
{
    Player_KeyInput _player_KeyInput;
    Player_KeyInput player_KeyInput
    {
        get
        {
            if (_player_KeyInput == null)
            {
                _player_KeyInput = GetComponent<Player_KeyInput>();
            }

            return _player_KeyInput;
        }
    }

    Player_Ani _player_Ani;
    Player_Ani player_Ani
    {
        get
        {
            if (_player_Ani == null)
            {
                _player_Ani = GetComponent<Player_Ani>();
            }
            return _player_Ani;
        }
    }

    Player_Attacker _player_Attacker;
    public Player_Attacker player_Attacker
    {
        get
        {
            if (_player_Attacker == null)
            {
                _player_Attacker = GetComponent<Player_Attacker>();
            }

            return _player_Attacker;
        }
    }

    float MiddleDelay;
    float MaxMiddleDelay = 0.25f;
    bool isMiddle;

    private void Start()
    {
        SetUp(1000000, 500, 10, IMovePoint.GetMovePoint(E_MoveData.Down));
    }

    private void Update()
    {
        UpdateMiddle();
    }


    public override void SetUp(int hp, float speed, int damage, Vector3 target)
    {
        base.SetUp(hp, speed, damage, target);
        nPC_ParticleSystem.ActiveParticle(E_ParticleKind.Running, 0);
        SetKeyInput();

        System.Action action = () =>
        {
            var kind = nPC_Move.GetMoveData() == E_MoveData.Down ? E_AniKind_Player.Running : E_AniKind_Player.Fly;
            player_Ani.SetAni(player_Ani.GetAniString(kind), true, null);
        };
        PlayManager.instance.AddAction(E_Play.Boss, action);
    }

    void SetKeyInput()
    {
        player_KeyInput.AddKeyPoint_Down(KeyCode.F, KeyDown_F);
        player_KeyInput.AddKeyPoint_Down(KeyCode.J, KeyDown_J);

        player_KeyInput.AddKeyPoint_Up(KeyCode.F, ResetKey);
        player_KeyInput.AddKeyPoint_Up(KeyCode.J, ResetKey);

        player_KeyInput.AddTwinKeyPoint_Down(KeyCode.F, KeyCode.J, SetTwin);
        player_KeyInput.AddTwinKeyPoint_Up(KeyCode.F, KeyCode.J, ResetKey);
    }

    void KeyDown_F()
    {
        if (!CheckMoveAni(E_MoveData.Up))
        {
            player_Ani.SetAni(player_Ani.GetAniString(E_AniKind_Player.Fly), true, null);
        }
        nPC_Move.SetTarget(E_MoveData.Up);
        player_Attacker.SetHold(true);
        player_Attacker.SetAttack(E_MoveData.Up);
    }

    void KeyDown_J()
    {
        if (!CheckMoveAni(E_MoveData.Down))
        {
            player_Ani.SetAni(player_Ani.GetAniString(E_AniKind_Player.Down), true, null);
        }
        nPC_Move.SetTarget(E_MoveData.Down);
        player_Attacker.SetHold(true);
        player_Attacker.SetAttack(E_MoveData.Down);
    }

    /// <summary>
    /// idle애니메이션 체크
    /// </summary>
    bool CheckMoveAni(E_MoveData data)
    {
        return nPC_Move.GetMoveData() == data;
    }

    /// <summary>
    /// 트윈 온
    /// </summary>
    void SetTwin()
    {
        player_Attacker.SetTwin(true);
        player_Attacker.SetAttack(E_MoveData.Twin);
    }

    /// <summary>
    /// 키보드 손 땠을때
    /// </summary>
    void ResetKey()
    {
        player_Attacker.SetTwin(false);
        player_Attacker.SetHold(false);
    }

    public override void SetAttack(NPC target)
    {
        //특수 몬스터 확인 후 공격
        var monstertype = target.GetComponent<IMonsterType>();
        var check = player_Attacker.CheckSpecialMonster(monstertype);
        if (!check)
        {
            return;
        }
        base.SetAttack(target);

        var point = target.nPC_Move.GetMoveData();
        var type = monstertype.GetMonsterType();
        player_Ani.SetAttackAni(type, point);
        if (type == E_MonsterType.Middle || type == E_MonsterType.Twin)
        {
            SetMiddle();
        }

        DamageEffect.Create(point);
        GameLog.Log($"공격력{nPC_Status.GetDamage()} / HP{target.nPC_Status.GetHp()}");
    }

    public override void SetHit(int hp)
    {
        Effect.Create(transform.position, (int)HitCollisionDetection.ConditionEffect.Opps);
        if (nPC_Status.CheckDie())
        {
            return;
        }
        ScoreManager.instance.SetScoreState(ScoreManager.E_ScoreState.Miss);
        ScoreManager.instance.SetCombo_Reset();
        player_Ani.SetAni(player_Ani.GetAniString(E_AniKind_Player.Hit), false, player_Ani.GetAniString(E_AniKind_Player.Idle));
        base.SetHit(hp);
        UpdateHp();
        print($"현재HP{nPC_Status.GetHp()}");
    }

    public override void SetDie()
    {
        base.SetDie();
        player_Ani.SetAni(player_Ani.GetAniString(E_AniKind_Player.Die), true, null);
        PlayManager.instance.SetAction(E_Play.End);
        print("사망");
    }

    /// <summary>
    /// 미들 이동 시 업데이트
    /// </summary>
    void UpdateMiddle()
    {
        if (!isMiddle)
        {
            return;
        }
        MiddleDelay += Time.deltaTime;

        if (MaxMiddleDelay > MiddleDelay)
        {
            return;
        }
        isMiddle = false;
        nPC_Move.SetTarget(E_MoveData.Down);
    }

    void SetMiddle()
    {
        MiddleDelay = 0;
        isMiddle = true;
        nPC_Move.SetTarget(E_MoveData.Middle);
    }

    void UpdateHp()
    {
        var max = nPC_Status.MaxHp;
        var cur = nPC_Status.GetHp();
        UI_Play.instance.SetHp(max, cur);
    }
}
