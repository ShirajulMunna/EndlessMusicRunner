using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class IPlayer_Attack : MonoBehaviour
{
    PlayerSystem _Player;
    PlayerSystem Player
    {
        get
        {
            if (_Player == null)
            {
                _Player = GetComponent<PlayerSystem>();
            }
            return _Player;
        }
    }

    IPlayer_KeyPoint State
    {
        get => Player.M_State;
    }

    //X값 오프셋
    const float OffSetX_value = 0.5f;

    //피격박스 사이즈
    List<Vector3> BoxSize = new List<Vector3>()
    {
        new Vector3(1.5f, 1f, 1),
        new Vector3(3f, 1f, 1),
        new Vector3(1f,1f,1),
        new Vector3(1f,1f,1)
    };


    //위치

    [HideInInspector]
    public List<Vector3> Tr_AttackVector = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> Tr_AttackVector_Low = new List<Vector3>();

    //공격 상태
    E_AttackState AttackState = E_AttackState.Attack_Re;

    private void Start()
    {
        Tr_AttackVector = new List<Vector3>()
        {
            new Vector3(-11, 2f, 0),
            new Vector3(-11, 3.5f, 0),
            new Vector3(-11, 5f, 0),
        };

        Tr_AttackVector_Low = new List<Vector3>()
        {
            new Vector3(-11, -4f, 0),
            new Vector3(-11, -2.5f, 0),
            new Vector3(-11, -1f, 0),
        };
    }

    //공격 함수
    public E_AttackState Attack(E_MovePoint point)
    {
        if (point == E_MovePoint.None)
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
            return E_AttackState.None;
        }

        var item = col[0];

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

    //히트 판정
    (Collider2D[], ScoreManager.E_ScoreState) SetHit(int idx)
    {
        var area = Area(Player);
        //퍼펙트 체크
        var col = Physics2D.OverlapBoxAll(area[idx], BoxSize[(int)ScoreManager.E_ScoreState.Perfect], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Perfect);
        }

        //그레이트 체크
        col = Physics2D.OverlapBoxAll(area[idx], BoxSize[(int)ScoreManager.E_ScoreState.Great], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Great);
        }

        //얼리 체크
        var earlypos = area[idx];
        earlypos.x += OffSetX_value;
        col = Physics2D.OverlapBoxAll(earlypos, BoxSize[(int)ScoreManager.E_ScoreState.Early], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Early);
        }

        //늦음 체크
        var latepos = area[idx];
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
        var posarea = Area(Player);
        var pos = posarea[i];
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

    private void OnDrawGizmos()
    {
        Tr_AttackVector = new List<Vector3>()
        {
            new Vector3(-11, 2f, 0),
            new Vector3(-11, 3.5f, 0),
            new Vector3(-11, 5f, 0),
        };

        Tr_AttackVector_Low = new List<Vector3>()
        {
            new Vector3(-11, -4f, 0),
            new Vector3(-11, -2.5f, 0),
            new Vector3(-11, -1f, 0),
        };
        for (int i = 0; i < 3; i++)
        {
            DrawOverlapBox(i, ScoreManager.E_ScoreState.Perfect, Color.green);

            DrawOverlapBox(i, ScoreManager.E_ScoreState.Early, Color.yellow);

            DrawOverlapBox(i, ScoreManager.E_ScoreState.Late, Color.red);

            DrawOverlapBox(i, ScoreManager.E_ScoreState.Great, Color.blue);
        }
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

    public List<Vector3> Area(PlayerSystem player)
    {
        var posarea = player.isHigt ? Tr_AttackVector : Tr_AttackVector_Low;
        return posarea;
    }
}