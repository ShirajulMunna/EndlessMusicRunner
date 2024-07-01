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
    float HitDelay;
    const float MaxHitDelay = 1f;
    int DirX = -1;

    private List<string> HitRandAnimation = new List<string>()
    {
        "Hit","Hit2","Hit3"
    };
    E_BossState e_BossState = E_BossState.Move;


    Vector3 TargetPos;

    private void Awake()
    {
        instance = this;
        TargetPos = StartPos;
    }

    protected override void Update()
    {
        base.Update();
        UpdateStaet();
    }

    void UpdateStaet()
    {
        print($"지금이순간?{e_BossState}");
        switch (e_BossState)
        {
            case E_BossState.Idle:
                break;
            case E_BossState.MoveAttack:
                base.SetMove(DirX);
                var check = CheckAttack();
                if (!check)
                {
                    return;
                }
                SetBossState(E_BossState.Idle);
                SetBossState(E_BossState.Move);
                break;
            case E_BossState.Move:
                print("마이 스피드 : " + Speed);
                base.SetMove(DirX);
                check = CheckTargetPos();
                if (!check)
                {
                    return;
                }
                transform.position = TargetPos;
                DoNotMoveGame();
                break;
            case E_BossState.Hit:
                HitDelay += Time.deltaTime;
                if (HitDelay < MaxHitDelay)
                {
                    return;
                }
                TargetPos = StartPos;
                SetBossState(E_BossState.Move);
                break;
            case E_BossState.Die:
                break;
        }
    }

    public override void SetMove(int dirx = -1)
    {
        Vector2 targetPos = TargetPos;
        Vector2 currentPos = rb.position;
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, Speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    public void SetBossState(E_BossState state)
    {
        switch (state)
        {
            case E_BossState.Idle:
                TargetPos = StartPos;
                GameManager.instance.player.SetisStop(true);
                break;
            case E_BossState.Move:
                SetZoomOut();
                Speed = 20;
                SetAni(E_BossAttack.idle);
                break;
            case E_BossState.MoveAttack:
                TargetPos = GameManager.instance.player.transform.position;
                TargetPos.y = StartPos.y;
                break;
            case E_BossState.Hit:
                HitDelay = 0;
                Speed = 0;
                DirX = 1;
                TargetPos = StartPos;
                SetZoomIn();
                int random = Random.Range(0, HitRandAnimation.Count - 1);
                skeletonAnimation.SetAni_Monster(HitRandAnimation[random], true);
                break;
            case E_BossState.Die:
                SetBossState(E_BossState.Idle);
                SetAni(E_BossAttack.Die);
                Destroy(gameObject, 1f);
                break;
        }
        e_BossState = state;
    }

    //타겟 위치에 도달 했는지 체크
    bool CheckTargetPos()
    {
        // 현재 위치와 StartPos 사이의 거리를 계산
        float distance = Vector3.Distance(transform.position, TargetPos);

        print($"내위치 : {transform.position} / 타겟위치{TargetPos}");

        // 거리가 임계값 이하이면 도달한 것으로 간주
        return distance <= 0.1f;
    }

    protected override void SetAttack(bool check)
    {
        base.SetAttack(check);

        if (check)
        {
            SetCrush();
        }
    }

    protected override bool CheckHitPoint()
    {
        return true;
    }

    void SetCrush()
    {
        GameManager.instance.player.SetHit();
        SetBossState(E_BossState.Move);
        DirX = -1;
    }

    public override void SetHit()
    {
        if (e_BossState == E_BossState.Move)
        {
            return;
        }
        base.SetHit();

        if (e_BossState == E_BossState.Hit)
        {
            return;
        }
        SetBossState(E_BossState.Hit);
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
        SetBossState(E_BossState.Idle);
    }

    IPlayer_Move PlayerMove
    {
        get => GameManager.instance.player.M_Move;
    }

    //줌인 단계
    void SetZoomIn()
    {
        PlayerMove.SetDirrectMove(E_MovePoint.Middle);
        CameraSystem.cameraSystem.SetZoomIn();
    }

    //줌아웃 단계
    void SetZoomOut()
    {
        PlayerMove.SetDirrectMove(E_MovePoint.Down);
        CameraSystem.cameraSystem.ReSetZoom();
    }
}