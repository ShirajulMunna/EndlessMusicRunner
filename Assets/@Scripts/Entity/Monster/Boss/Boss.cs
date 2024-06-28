using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Boss : Monster
{
    public static Boss instance;

    [Space(10f)]
    [Header("보스 공격 관련ㅡㅡㅡㅡ")]
    List<string> L_Ani = new List<string>()
    {
        "idle",
        "idle2",
        "Attack1",
        "Attack2",
        "Attack3",
        "Hit",
        "retire",
        "Start"
    };

    [Header("보스 커스텀 공격 위치")]
    public Transform[] Tr_CustomCreate;

    //6.14 보스 바닥에서 나오게하는 코드 작업
    Vector3 StartPos = new Vector3(13.5f, -4.2f, 0);

    bool hasReachedStartPos = false;
    bool isDestorySetting = false;

    bool isBossDie = false;

    float DieDelay = 2f;
    private void Awake()
    {
        instance = this;
    }

    protected override void Update()
    {
        base.Update();

        //보스 뒤로가게만듬
        if (SpawnManager.instance.GetGameState() == E_GameState.End)
        {
            DieDelay -= Time.deltaTime;
            if (DieDelay > 0)
            {
                return;
            }

            if (!isBossDie)
            {
                isBossDie = true;
                SetAni(E_BossAttack.Die);
                Destroy(gameObject, 1f);
            }
        }
    }

    public override void SetMove()
    {
        if (hasReachedStartPos)
        {
            GameManager.instance.player.SetisStop(true);
            return;
        }

        base.SetMove();

        // StartPos에 도달했는지 체크
        if (transform.position.x <= StartPos.x)
        {
            // 위치를 고정하고 SetMove를 더 이상 호출하지 않도록 설정
            transform.position = StartPos;
            hasReachedStartPos = true;
            DoNotMoveGame();
        }
    }

    //애니셋팅
    public void SetAni(E_BossAttack boss)
    {
        var str = GetBossAni(boss);
        skeletonAnimation.SetAni_Monster(str, default, L_Ani[(int)E_BossAttack.idle2]);
    }
    //애니메이션 딜레이 확인
    public float GetAniDelay(E_BossAttack boss)
    {
        var str = GetBossAni(boss);
        return skeletonAnimation.GetAniDelay(str);
    }

    //애니메이션 가져오기
    string GetBossAni(E_BossAttack boss)
    {
        return L_Ani[(int)boss];
    }

    public Transform[] GetAttackPoint()
    {
        return Tr_CustomCreate;
    }

    //백그라운드 멈춰있을때 스테이지 정보가 1000이상이면 백그라운드 멈춰있는것으로 간주
    public void DoNotMoveGame()
    {
        var str = L_Ani[(int)E_BossAttack.Start];
        skeletonAnimation.SetAni_Monster(str, false, L_Ani[(int)E_BossAttack.idle2]);
    }
}