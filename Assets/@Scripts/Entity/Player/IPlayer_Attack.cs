using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class IPlayer_Attack
{
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

    //현재 공격 위치
    [SerializeField] E_AttackPoint AttackPoint = E_AttackPoint.Down;

    bool isTwinAttack = false;
    PlayerSystem player
    {
        get => GameManager.instance.player;
    }

    //공격 카운트 수정
    float AttackDelay = 2f;

    //현재 공격 횟수
    int AttackCount = 0;

    //홀딩 딜레이
    float HoldDelay = 0.2f;

    public E_AttackPoint Attack()
    {
        AttackDelay -= Time.deltaTime;
        HoldDelay -= Time.deltaTime;
        E_AttackPoint point = E_AttackPoint.None;
        if (Input.GetKey(KeyCode.F) && CheckAttackState())
        {
            point = UpAttack();
            //동시 키 입력처리
            if (Input.GetKey(KeyCode.J))
            {
                isTwinAttack = true;
                Debug.Log(" F And J");
            }
        }
        if (Input.GetKey(KeyCode.J) && CheckAttackState())
        {
            point = DownAttack();
            //동시 키 입력처리
            if (Input.GetKey(KeyCode.F))
            {
                isTwinAttack = true;
                Debug.Log(" J And F");
            }
        }
        if (AttackState == E_AttackState.Hold)
        {
            if (HoldDelay <= 0)
            {
                HoldDelay = 0.2f;
            }
        }

        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.J))
        {
            Reset();
        }

        return point;
    }


    //상단 공격
    E_AttackPoint UpAttack()
    {
        var idx = SetAttack_Idx(E_AttackPoint.Up, E_AttackPoint.Down);
        AttackState = SetAttack(idx.Item1);
        if (HoldDelay <= 0)
        {
            var state = AttackState == E_AttackState.None ? E_AniType.Fly : E_AniType.Fire_Attack;
            player.SetAni(player.GetAniName(state));
        }
        player.SetParticle(E_PlayerSkill.Fly, 0);
        return idx.Item2;
    }


    //하단공격
    E_AttackPoint DownAttack()
    {
        var idx = SetAttack_Idx(E_AttackPoint.Down, E_AttackPoint.Up);
        AttackState = SetAttack(idx.Item1);
        if (HoldDelay <= 0)
        {
            var type = AttackCount > 1 ? E_AniType.Tail_Attack : E_AniType.Kick;
            var state = AttackState == E_AttackState.Hold ? E_AniType.Hold_Attack : type;
            player.SetAni(player.GetAniName(state));
        }
        player.SetParticle(E_PlayerSkill.Running, 0);
        return idx.Item2;
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

    //공격 상태 변경
    (E_AttackPoint, E_AttackPoint) SetAttack_Idx(E_AttackPoint nextpoint, E_AttackPoint checkpoint)
    {
        AttackCount++;
        if (CheckAttackPoin(checkpoint) || AttackDelay <= 0)
        {
            AttackCount = 0;
        }
        AttackDelay = 2;
        // 검사해야할 조건을추가하여 다음 위치로 가게 만듬 
        if (CheckAttackPoin(E_AttackPoint.Middle))
        {
            return (nextpoint, E_AttackPoint.Middle);
        }
        SetDirectMoveIdx(nextpoint);
        return (nextpoint, nextpoint);
    }

    //상태 비교 체크
    public bool CheckAttackPoin(E_AttackPoint state)
    {
        return AttackPoint == state;
    }

    //공격 위치 셋팅
    public void SetDirectMoveIdx(E_AttackPoint idx)
    {
        AttackPoint = idx;
    }

    //리셋
    void Reset()
    {
        AttackState = E_AttackState.Attack_Re;
    }

    //공격 함수
    E_AttackState SetAttack(E_AttackPoint attackidx)
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

            if (result)
            {
                return E_AttackState.Attack;
            }

            //롱 노트 일때 처리
            var longnote = item.GetComponent<LongNote>();
            result = SetLongNote(longnote, perfect);

            if (result)
            {
                return E_AttackState.Hold;
            }

            //2단 몬스터 일때 
            var twinMonster = item.GetComponent<TwinMonster>();
            result = SetTwinMonster(twinMonster, perfect);
            if (result)
            {
                return E_AttackState.Attack;
            }

            //몬스터 일때 처리
            var monster = item.GetComponent<Monster>();
            result = SetMonster(monster, perfect);

            if (result)
            {
                return E_AttackState.Attack;
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
    bool SetMonster(Monster monster, ScoreManager.E_ScoreState perfect)
    {
        //2단몬스터가 몬스터를 상속받아서 판정이 결국 몬스터까지 오는 문제가 있음
        if (monster.GetComponent<TwinMonster>() != null)
        {
            return false;
        }
        if (monster == null)
        {
            return false;
        }
        monster.SetHit(perfect);
        return true;
    }

    //보스 공격 셋팅
    bool SetBoss(Boss boss, ScoreManager.E_ScoreState perfect)
    {
        if (boss == null)
        {
            return false;
        }
        boss.SetHit(perfect);
        return true;
    }

    //롱노트 셋팅
    bool SetLongNote(LongNote longnote, ScoreManager.E_ScoreState perfect)
    {
        if (longnote == null)
        {
            return false;
        }
        longnote.SetAttack(perfect);
        return true;
    }
    //2단몬스터 검사
    bool SetTwinMonster(TwinMonster twinMonster, ScoreManager.E_ScoreState perfect)
    {
        if (twinMonster == null || !isTwinAttack)
        {
            return false;
        }
        twinMonster.SetHit(perfect);
        isTwinAttack = false;
        return true;
    }
}