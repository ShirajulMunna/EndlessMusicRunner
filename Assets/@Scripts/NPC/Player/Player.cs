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
    Player_Attacker player_Attacker
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

    private void Start()
    {
        SetUp(100, 500, 10, IMovePoint.GetMovePoint(E_MoveData.Down));
    }

    public override void SetUp(int hp, float speed, int damage, Vector3 target)
    {
        base.SetUp(hp, speed, damage, target);
        nPC_ParticleSystem.ActiveParticle(E_ParticleKind.Running);
        SetKeyInput();
    }

    void SetKeyInput()
    {
        player_KeyInput.AddKeyPoint_Down(KeyCode.F, KeyDown_F);
        player_KeyInput.AddKeyPoint_Down(KeyCode.J, KeyDown_J);
        player_KeyInput.AddKeyPoint_Up(KeyCode.F, ResetKey);
        player_KeyInput.AddKeyPoint_Up(KeyCode.J, ResetKey);
        player_KeyInput.AddTwinKeyPoint(KeyCode.F, KeyCode.J, SetTwin);
    }

    void KeyDown_F()
    {
        nPC_Move.SetTarget(E_MoveData.Up);
        player_Attacker.SetHold(true);
        player_Attacker.SetAttack(E_MoveData.Up);
        player_Ani.SetAni(player_Ani.GetAniString(E_AniKind_Player.Fly), true, null);
    }

    void KeyDown_J()
    {
        nPC_Move.SetTarget(E_MoveData.Down);
        player_Attacker.SetHold(true);
        player_Attacker.SetAttack(E_MoveData.Down);
        player_Ani.SetAni(player_Ani.GetAniString(E_AniKind_Player.Down), true, null);
    }

    /// <summary>
    /// 트윈 온
    /// </summary>
    void SetTwin()
    {
        player_Attacker.SetTwin(true);
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
        player_Ani.SetAttackAni(point);

        var type = monstertype.GetMonsterType();
        if (type == E_MonsterType.Middle || type == E_MonsterType.Twin)
        {
            nPC_Move.SetTarget(E_MoveData.Middle);
        }
    }
}
