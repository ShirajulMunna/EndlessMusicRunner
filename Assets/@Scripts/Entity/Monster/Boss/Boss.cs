using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : Monster
{
    public static Boss boss;

    public static void Create(Vector3 pos)
    {
        var load = Resources.Load<GameObject>("Boss");
        var boss = Instantiate<GameObject>(load);
        boss.transform.position = pos;
    }

    public List<string> L_Ani = new List<string>()
    {
        "idle",
        "Attack1",
        "Attack2",
        "Attack3",
        "Hit",
        "retire",
    };


    private void Awake()
    {
        boss = this;
    }

    //애니셋팅
    public void SetAni(E_BossAttack boss)
    {
        var str = GetBossAni(boss);
        skeletonAnimation.SetAni_Monster(str);
    }

    //애니메이션 가져오기
    string GetBossAni(E_BossAttack boss)
    {
        return L_Ani[(int)boss];
    }
}