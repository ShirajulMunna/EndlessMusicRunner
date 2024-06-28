using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Monster_Hammer : Monster
{
    [SerializeField] float Delay_Ani;

    System.Action Ac_SetActive;

    public override void SetUp(C_MonsterTable data, Vector3 cratepos)
    {
        base.SetUp(data, cratepos);
        skeletonAnimation.gameObject.SetActive(false);
        Ac_SetActive += SetActive;
    }

    protected override void Update()
    {
        base.Update();
        Ac_SetActive?.Invoke();
    }


    void SetActive()
    {
        Delay_Ani -= Time.deltaTime;

        if (Delay_Ani > 0)
        {
            return;
        }
        Ac_SetActive = null;
        skeletonAnimation.gameObject.SetActive(true);
    }

    public override void SetDie()
    {
        base.SetDie();
        skeletonAnimation.SetAni_Monster("Hit", true);
        Destroy(skeletonAnimation.transform.parent.gameObject, 0.5f);
    }
    public override void SetMove(int dirx = -1)
    {
    }
}
