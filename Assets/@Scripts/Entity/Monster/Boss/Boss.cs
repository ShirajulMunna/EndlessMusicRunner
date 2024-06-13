using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : Monster
{
    public static Boss instance;

    public List<string> L_Ani = new List<string>()
    {
        "idle",
        "Attack1",
        "Attack2",
        "Attack3",
        "Hit",
        "retire",
    };

    Vector3 StartPos = new Vector3(10, 0, 0);

    bool hasReachedStartPos = false;

    private void Awake()
    {
        instance = this;
    }


    public override void SetMove()
    {
        if (hasReachedStartPos)
        {
            return;
        }

        base.SetMove();

        // StartPos에 도달했는지 체크
        if (transform.position.x <= StartPos.x)
        {
            // 위치를 고정하고 SetMove를 더 이상 호출하지 않도록 설정
            transform.position = StartPos;
            hasReachedStartPos = true;
        }
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