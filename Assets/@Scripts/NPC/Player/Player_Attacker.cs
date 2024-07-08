using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attacker : MonoBehaviour, IPlayerAttack
{
    NPC _nPC_Status;
    public NPC nPC
    {
        get
        {
            if (_nPC_Status == null)
            {
                _nPC_Status = GetComponent<NPC>();
            }

            return _nPC_Status;
        }
        set => _nPC_Status = value;
    }

    Player_Effect _player_Effect;
    public Player_Effect player_Effect
    {
        get
        {
            if (_player_Effect == null)
            {
                _player_Effect = GetComponent<Player_Effect>();
            }

            return _player_Effect;
        }
    }

    public bool isTwin { get; set; }
    public bool isHold { get; set; }
    //X값 오프셋
    public float OffSetX_value { get; set; } = 0.5f;
    //피격박스 사이즈
    public List<Vector3> BoxSize { get; set; } = new List<Vector3>()
    {
        new Vector3(1.5f, 1.5f, 1),
        new Vector3(3f, 1.5f, 1),
        new Vector3(1f,1.5f,1),
        new Vector3(1f,1.5f,1)
    };

    //위치
    public Dictionary<E_MoveData, Vector3> Tr_AttackVector { get; set; } = new Dictionary<E_MoveData, Vector3>()
    {
        {E_MoveData.Down,new Vector3(-11, -3.5f, 0)},
        {E_MoveData.Middle,new Vector3(-11, 0f, 0)},
        {E_MoveData.Up,new Vector3(-11, 3.5f, 0)},
        {E_MoveData.Twin,new Vector3(-11, 0f, 0)},
    };

    public void SetAttack(E_MoveData idx)
    {
        var result = SetAttack_Area(idx);

        if (result.Item1 == null)
        {
            return;
        }
        var obj = result.Item1[0].gameObject;
        var target = obj.GetComponent<NPC>();
        nPC.SetAttack(target);

        SetMonsterEffect(obj, idx, result.Item2);
    }

    /// <summary>
    /// 이펙트 처리
    /// </summary>
    public void SetMonsterEffect(GameObject obj, E_MoveData idx, ScoreManager.E_ScoreState scorestate)
    {
        var mon = obj.GetComponent<IMonster>();
        var type = mon.monsterType.GetMonsterType();

        var twin = type == E_MonsterType.Twin && idx != E_MoveData.Twin;
        var mid = type == E_MonsterType.Middle && idx != E_MoveData.Middle;
        if (twin || mid)
        {
            return;
        }

        player_Effect.SetEffect(Tr_AttackVector[idx], scorestate);
    }

    public (Collider2D[], ScoreManager.E_ScoreState) SetAttack_Area(E_MoveData idx)
    {
        GameLog.Log($"위치값: {Tr_AttackVector[idx]} /방향: {idx}");
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

    /// <summary>
    /// 홀드 변환
    /// </summary>
    public void SetHold(bool state)
    {
        isHold = state;
    }

    public bool CheckHold()
    {
        return isHold;
    }

    /// <summary>
    /// 트윈 변환
    /// </summary>
    public void SetTwin(bool state)
    {
        isTwin = state;
    }

    public bool CheckTwin()
    {
        return isTwin;
    }

    /// <summary>
    /// 특수 몬스터 처리 확인 
    /// </summary>
    public bool CheckSpecialMonster(IMonsterType types)
    {
        switch (types.e_MonsterType)
        {
            case E_MonsterType.Twin:
                return CheckTwin();
            case E_MonsterType.Hold:
                return CheckHold();
            default:
                return true;
        }
    }


    #region  박스 그리기

#if UNITY_EDITOR

    void DrawOverlapBox(E_MoveData i, ScoreManager.E_ScoreState state, Color color)
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

    private void OnDrawGizmos()
    {
        foreach (E_MoveData item in Enum.GetValues(typeof(E_MoveData)))
        {
            DrawOverlapBox(item, ScoreManager.E_ScoreState.Perfect, Color.green);

            DrawOverlapBox(item, ScoreManager.E_ScoreState.Early, Color.yellow);

            DrawOverlapBox(item, ScoreManager.E_ScoreState.Late, Color.red);

            DrawOverlapBox(item, ScoreManager.E_ScoreState.Great, Color.blue);
        }
    }
#endif
    #endregion
}

