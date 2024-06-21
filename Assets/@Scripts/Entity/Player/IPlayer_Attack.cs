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

    IPlayer_KeyPoint State
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
        new Vector3(-11, -3.5f, 0),
        new Vector3(-11, 0, 0),
        new Vector3(-11, 3.5f, 0)
    };
    //공격 상태
    E_AttackState AttackState = E_AttackState.Attack_Re;

    //공격 함수
    public E_AttackState Attack(E_MovePoint point)
    {
        if (point == E_MovePoint.None)
        {
            return E_AttackState.None;
        }

        var check = CheckAttackState();
        if (!check)
        {
            return E_AttackState.None;
        }
        AttackState = SetAttack(point);
        return AttackState;
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
            //바닥에선 공격가능하게 만듬
            if(attackidx == E_MovePoint.Down) 
            {
                return E_AttackState.Attack;
            }
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
        twinMonster.SetHit(perfect);
        return E_AttackState.Twin_Attack;
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