using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
[Serializable]
public class IPlayer_Attack
{
    PlayerSystem Player
    {
        get => GameManager.instance.player;
    }

    IPlayer_State State
    {
        get => GameManager.instance.player.M_State;
    }

    //X값 오프셋
    const float OffSetX_value = 0.5f;

    //피격박스 사이즈
    List<Vector3> BoxSize = new List<Vector3>()
    {
        new Vector3(1.5f, 1.5f, 1),
        new Vector3(3f, 1.5f, 1),
        new Vector3(1f,1.5f,1),
        new Vector3(1f,1.5f,1)
    };

    //위치
    public List<Vector3> Tr_AttackVector = new List<Vector3>()
    {
        new Vector3(-7, -3.5f, 0),
        new Vector3(-7, 0, 0),
        new Vector3(-7, 3.5f, 0)
    };
    //공격 상태
    E_AttackState AttackState = E_AttackState.Attack_Re;


    //현재 공격 횟수
    int AttackCount = 0;
    const int MaxAttackCount = 2;

    //홀딩 딜레이
    float HoldDelay;
    const float MaxHoldDelay = 0.3f;


    //공격 함수
    public void Attack(E_MovePoint point)
    {
        HoldDelay -= Time.deltaTime;

        if (point == E_MovePoint.None)
        {
            return;
        }

        var check = CheckAttackState();
        if (!check)
        {
            return;
        }

        var types = (E_PlayerSkill.Running, E_AniType.Kick, E_AniType.Tail_Attack);

        switch (point)
        {
            case E_MovePoint.Up:
                types = (E_PlayerSkill.Fly, E_AniType.Fly_Attack, E_AniType.Fire_Attack);
                break;
            case E_MovePoint.Down:
            case E_MovePoint.Middle:
                break;
        }
        SetAttackCount();
        SetAni_Particle(types);
        SetHold();
        AttackState = SetAttack(point);
        return;
    }


    //홀드일 경우 처리
    void SetHold()
    {
        if (!CheckHoldPoint())
        {
            return;
        }

        if (HoldDelay <= 0)
        {
            HoldDelay = MaxHoldDelay;
        }
    }

    //공격 함수
    E_AttackState SetAttack(E_MovePoint attackidx)
    {
        var result_hit = SetHit((int)attackidx);

        var col = result_hit.Item1;
        var perfect = result_hit.Item2;

        //허공에 공격   
        if (col == null)
        {
            return E_AttackState.None;
        }

        foreach (var item in col)
        {
            //보스일때 처리
            var boss = item.GetComponent<Boss>();
            var result = SetBoss(boss, perfect);

            if (result != E_AttackState.None)
            {
                return result;
            }

            //롱 노트 일때 처리
            var longnote = item.GetComponent<Monster_LongNote>();
            result = SetLongNote(longnote, perfect);

            if (result != E_AttackState.None)
            {
                return result;
            }

            //2단 몬스터 일때 
            var twinMonster = item.GetComponent<Monster_Twin>();
            result = SetTwinMonster(twinMonster, perfect);
            if (result != E_AttackState.None)
            {
                return result;
            }

            //몬스터 일때 처리
            var monster = item.GetComponent<Monster>();
            result = SetMonster(monster, perfect);

            if (result != E_AttackState.None)
            {
                return result;
            }

            return E_AttackState.None;
        }

        return E_AttackState.None;
    }

    //상단 공격
    void SetAni_Particle((E_PlayerSkill skill, E_AniType zero, E_AniType one) data)
    {
        if (HoldDelay <= 0)
        {
            var type = AttackCount == 0 ? data.zero : data.one;
            var state = AttackState == E_AttackState.Hold ? E_AniType.Hold_Attack : type;
            Player.SetAni(Player.GetAniName(state));
        }
        Player.SetParticle(data.skill, 0);
    }

    //하단공격
    void DownAttack()
    {
        if (HoldDelay <= 0)
        {
            var type = AttackCount == 0 ? E_AniType.Kick : E_AniType.Tail_Attack;
            var state = AttackState == E_AttackState.Hold ? E_AniType.Hold_Attack : type;
            Player.SetAni(Player.GetAniName(state));
        }
        Player.SetParticle(E_PlayerSkill.Running, 0);
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

    //히트 판정
    (Collider2D[], ScoreManager.E_ScoreState) SetHit(int idx)
    {
        //퍼펙트 체크
        var col = Physics2D.OverlapBoxAll(Tr_AttackVector[idx], BoxSize[(int)ScoreManager.E_ScoreState.Perfect], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Perfect);
        }

        //그레이트 체크
        col = Physics2D.OverlapBoxAll(Tr_AttackVector[idx], BoxSize[(int)ScoreManager.E_ScoreState.Great], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Great);
        }

        //얼리 체크
        var earlypos = Tr_AttackVector[idx];
        earlypos.x += OffSetX_value;
        col = Physics2D.OverlapBoxAll(earlypos, BoxSize[(int)ScoreManager.E_ScoreState.Early], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Early);
        }

        //늦음 체크
        var latepos = Tr_AttackVector[idx];
        latepos.x += -OffSetX_value;
        col = Physics2D.OverlapBoxAll(latepos, BoxSize[(int)ScoreManager.E_ScoreState.Late], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Late);
        }

        return (null, ScoreManager.E_ScoreState.Miss);
    }

    public void DrawOverlapBox(int i, ScoreManager.E_ScoreState state, Color color)
    {
        var pos = Tr_AttackVector[i];
        var boxsize = BoxSize[(int)state];

        if (state == ScoreManager.E_ScoreState.Early)
        {
            pos.x += OffSetX_value;
        }
        else if (state == ScoreManager.E_ScoreState.Late)
        {
            pos.x += -OffSetX_value;
        }

        Gizmos.color = color;
        Gizmos.DrawWireCube(pos, boxsize);
    }

    //몬스터 공격 세팅
    E_AttackState SetMonster(Monster monster, ScoreManager.E_ScoreState perfect)
    {
        if (monster == null)
        {
            return E_AttackState.None;
        }
        monster.SetHit(perfect);
        return E_AttackState.Attack;
    }

    //보스 공격 셋팅
    E_AttackState SetBoss(Boss boss, ScoreManager.E_ScoreState perfect)
    {
        if (boss == null)
        {
            return E_AttackState.None;
        }
        boss.SetHit(perfect);
        return E_AttackState.Attack;
    }

    //롱노트 셋팅
    E_AttackState SetLongNote(Monster_LongNote longnote, ScoreManager.E_ScoreState perfect)
    {
        if (longnote == null)
        {
            return E_AttackState.None;
        }
        longnote.SetAttack(perfect);
        return E_AttackState.Hold;
    }
    //2단몬스터 검사
    E_AttackState SetTwinMonster(Monster_Twin twinMonster, ScoreManager.E_ScoreState perfect)
    {
        if (twinMonster == null)
        {
            return E_AttackState.None;
        }
        if (!State.CheckTwin())
        {
            return E_AttackState.Attack_Re;
        }
        Player.SetAni(("Twin_Attack", false));
        twinMonster.SetHit(perfect);
        return E_AttackState.Attack;
    }


    //리셋
    public void Reset()
    {
        AttackState = E_AttackState.Attack_Re;
        State.SetTwin(false);
    }

    //CheckHold
    public bool CheckHoldPoint()
    {
        return AttackState == E_AttackState.Hold;
    }


    //상태 비교 체크
    public bool GetAttackState(E_AttackState state)
    {
        return AttackState == state;
    }


    //공격 가능 상태 체크
    bool CheckAttackState()
    {
        return AttackState == E_AttackState.Hold || AttackState == E_AttackState.Attack_Re;
    }


}