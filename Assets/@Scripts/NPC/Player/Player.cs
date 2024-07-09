using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    bool isHigt;

    public System.Action Ac_Hit;

    private void Update()
    {
        UpdateMiddle();
    }

    public override void SetUp(int hp, float speed, int damage, Vector3 target)
    {
        base.SetUp(hp, speed, damage, target);
        nPC_ParticleSystem.ActiveParticle(E_ParticleKind.Running, 0);
        System.Action action = () =>
        {
            SetBoss();
        };
        PlayManager.instance.AddAction(E_Play.Boss, action);
    }

    void SetBoss()
    {
        var point = Getnpc().nPC_Move.GetMoveData();
        var kind = point == E_MoveData.Higt_Low || point == E_MoveData.Low_Low ? E_AniKind_Player.Running : E_AniKind_Player.Fly;
        GetAni_npc().SetAni(GetAni_npc().GetAniString(kind), true, null);
    }

    public void SetKeyInput(KeyCode code_1, KeyCode code_2, bool ishigt)
    {
        isHigt = ishigt;
        var up = ishigt ? E_MoveData.Higt_Higt : E_MoveData.Low_Higt;
        var downs = ishigt ? E_MoveData.Higt_Low : E_MoveData.Low_Low;
        var twin = ishigt ? E_MoveData.Higt_Twin : E_MoveData.Low_Twin;

        player_KeyInput.AddKeyPoint_Down(code_1, () => KeyDown(up, E_AniKind_Player.Fly));
        player_KeyInput.AddKeyPoint_Down(code_2, () => KeyDown(downs, E_AniKind_Player.Down));
        player_KeyInput.AddKeyPoint_Up(code_1, () => ResetKey());
        player_KeyInput.AddKeyPoint_Up(code_2, () => ResetKey());
        player_KeyInput.AddTwinKeyPoint_Down(code_1, code_2, () => SetTwin(twin));
        player_KeyInput.AddTwinKeyPoint_Up(code_1, code_2, () => ResetKey());

        SetUp(100, 500, 10, IMovePoint.GetMovePoint(downs));
    }

    void KeyDown(E_MoveData e_MoveData, E_AniKind_Player e_AniKind_Player)
    {
        var ani = GetAni_npc();
        var attacker = GetAttacker_npc();
        var nPc = Getnpc();

        var beforemove = nPc.nPC_Move.GetMoveData();
        nPc.nPC_Move.SetTarget(e_MoveData);

        if (!CheckMoveAni(beforemove, e_MoveData))
        {
            ani.SetAni(ani.GetAniString(e_AniKind_Player), true, null);
        }
        attacker.SetHold(true);
        attacker.SetAttack(e_MoveData);
    }
    /// <summary>
    /// idle애니메이션 체크
    /// </summary>
    bool CheckMoveAni(E_MoveData before, E_MoveData data)
    {
        return before == data;
    }

    public Player_Ani GetAni_npc()
    {
        return player_Ani;
    }

    public Player_Attacker GetAttacker_npc()
    {
        return player_Attacker;
    }

    public NPC Getnpc()
    {
        return this;
    }

    /// <summary>
    /// 트윈 온
    /// </summary>
    void SetTwin(E_MoveData e_MoveData)
    {
        var attacker = GetAttacker_npc();
        attacker.SetTwin(true);
        attacker.SetAttack(e_MoveData);
    }

    /// <summary>
    /// 키보드 손 땠을때
    /// </summary>
    void ResetKey()
    {
        var attacker = GetAttacker_npc();
        attacker.SetTwin(false);
        attacker.SetHold(false);
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
        Effect.Create(transform.position, 3);
        if (nPC_Status.CheckDie())
        {
            return;
        }
        Ac_Hit?.Invoke();
        ScoreManager.instance.SetScoreState(ScoreManager.E_ScoreState.Miss);
        ScoreManager.instance.SetCombo_Reset();
        player_Ani.SetAni(player_Ani.GetAniString(E_AniKind_Player.Hit), false, player_Ani.GetAniString(E_AniKind_Player.Idle));
        base.SetHit(hp);
        PlayerManager.instance.SetHit();
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
        nPC_Move.SetTarget(isHigt ? E_MoveData.Higt_Low : E_MoveData.Low_Low);
    }

    void SetMiddle()
    {
        MiddleDelay = 0;
        isMiddle = true;
        nPC_Move.SetTarget(isHigt ? E_MoveData.Higt_Middle : E_MoveData.Low_Middle);
    }

    void UpdateHp()
    {
        var max = nPC_Status.MaxHp;
        var cur = nPC_Status.GetHp();
        UI_Play.instance.SetHp(max, cur);
    }
}
