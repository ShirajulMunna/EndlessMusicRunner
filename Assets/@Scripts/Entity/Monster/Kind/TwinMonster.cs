using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinMonster : Monster
{
    IPlayer_State player_State
    {
        get => GameManager.instance.player.M_State;
    }

    const string Name = "Monster_{0}";

    protected override void Update()
    {
        base.Update();
    }

    protected override void SetAttack(bool check)
    {
        if (!check)
        {
            return;
        }
        e_MonsterState = E_MonsterState.NoneAttack;
        SetComboReset();
        CreatPlayerHitEffect();
    }

    public override void SetMinusHp(int value)
    {
        base.SetMinusHp(value);
        player_State.SetDirectMoveIdx(E_MovePoint.Middle);
    }
}
