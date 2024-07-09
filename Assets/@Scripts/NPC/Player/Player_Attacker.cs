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
    public Dictionary<E_MoveData, Vector3> Tr_AttackVector { get; set; } = IMovePoint.GetPoint();
    const float XVlaue = -11;

    Vector3 GetPoint(E_MoveData e_MoveData)
    {
        if (!Tr_AttackVector.ContainsKey(e_MoveData))
        {
            return default;
        }

        var pos = Tr_AttackVector[e_MoveData];
        pos.x = XVlaue;
        return pos;
    }


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

        ScoreManager.instance.SetScoreState(result.Item2);
        ScoreManager.instance.SetCurrentScore(1);
        ScoreManager.instance.SetCombo_Add();
    }

    /// <summary>
    /// 이펙트 처리
    /// </summary>
    public void SetMonsterEffect(GameObject obj, E_MoveData idx, ScoreManager.E_ScoreState scorestate)
    {
        var mon = obj.GetComponent<IMonster>();
        var type = mon.monsterType.GetMonsterType();

        if (type == E_MonsterType.Twin)
        {
            var check = idx == E_MoveData.Higt_Twin || idx == E_MoveData.Low_Twin;
            if (!check)
            {
                return;
            }
        }

        if (type == E_MonsterType.Middle)
        {
            var check = idx == E_MoveData.Higt_Middle || idx == E_MoveData.Low_Middle;
            if (!check)
            {
                return;
            }
        }
        
        AudioManager.instance.PlaySound(scorestate);
        player_Effect.SetEffect(GetPoint(idx), scorestate);
    }

    public (Collider2D[], ScoreManager.E_ScoreState) SetAttack_Area(E_MoveData idx)
    {
        GameLog.Log($"위치값: {GetPoint(idx)} /방향: {idx}");
        //퍼펙트 체크
        var col = Physics2D.OverlapBoxAll(GetPoint(idx), BoxSize[(int)ScoreManager.E_ScoreState.Perfect], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Perfect);
        }

        //그레이트 체크
        col = Physics2D.OverlapBoxAll(GetPoint(idx), BoxSize[(int)ScoreManager.E_ScoreState.Great], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Great);
        }

        //얼리 체크
        var earlypos = GetPoint(idx);
        earlypos.x += OffSetX_value;
        col = Physics2D.OverlapBoxAll(earlypos, BoxSize[(int)ScoreManager.E_ScoreState.Early], default);

        if (col != null && col.Length > 0)
        {
            return (col, ScoreManager.E_ScoreState.Early);
        }

        //늦음 체크
        var latepos = GetPoint(idx);
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
        var pos = GetPoint(i);
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

