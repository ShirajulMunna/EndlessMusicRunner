using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinMonster : Monster
{
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
}
