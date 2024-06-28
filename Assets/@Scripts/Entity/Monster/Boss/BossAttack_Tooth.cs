using UnityEngine;

public class BossAttack_Tooth : Monster
{
    const string SoundName = "Boss_Attack_Sound_{0}";

    [Space(10f)]
    [Header("보스 공격 관련ㅡㅡㅡㅡ")]

    [SerializeField] E_BossAttack BossState;
    [SerializeField] int PlaySound;

    [Header("보스 근처에서 생성")]
    [SerializeField] bool CheckCustomMove;
    [SerializeField] int CustomePos_IDX;
    Vector3 OriginPos;
    int isCount = 2;

    protected override void Update()
    {
        base.Update();
        SetAni_Sound();
    }

    public override void SetUp(C_MonsterTable data, Vector3 cratepos)
    {
        base.SetUp(data, cratepos);
        SetCustomPos(cratepos);
        Speed = 10;
    }

    //애니메이션 및 사운드 
    void SetAni_Sound()
    {
        if (isCount > 0)
        {
            isCount--;
            if (isCount <= 0)
            {
                Boss.instance.SetAni(BossState);
                SetSound();
            }
        }
    }


    //커스텀 이동 함수
    void SetCustomPos(Vector3 cratepos)
    {
        OriginPos = cratepos;
        if (CheckCustomMove)
        {
            var offsetx = OriginPos.x - 20;
            var pos = Boss.instance.GetAttackPoint()[CustomePos_IDX].position;
            pos.x += offsetx;
            transform.position = pos;
        }
    }

    //보스 공격 관련 사운드
    void SetSound()
    {
        if (PlaySound <= 0)
        {
            return;
        }

        AudioManager.instance.PlayEffectSound(string.Format(SoundName, PlaySound));
    }

    public override void SetMove()
    {
        if (CheckCustomMove)
        {
            SetCustomMove();
        }
        else
        {
            // y 값이 맞춰졌으면 x 방향으로만 이동
            base.SetMove();
        }
    }


    //특정 이동
    void SetCustomMove()
    {
        var targetpos = OriginPos;

        if (transform.position.y >= targetpos.y)
        {
            targetpos.x = transform.position.x;
            transform.position = targetpos;
            CheckCustomMove = false;
            return;
        }
        targetpos.x = player.transform.position.x;
        targetpos.y = OriginPos.y * 3;
        transform.position = Vector2.MoveTowards(transform.position, targetpos, Speed * Time.deltaTime);
    }

    public override void SetHit(ScoreManager.E_ScoreState perfect)
    {
        base.SetAttack(true);
    }

    protected override void SetAttack(bool check)
    {
        if (!check)
        {
            return;
        }

        //플레이어 위치에 있다면 히트
        var checkhit = CheckHitPoint();
        if (checkhit)
        {
            base.SetAttack(true);
            return;
        }

        //아니라면 사망
        SetDie();
    }

    public override void SetDie()
    {
        if (e_MonsterState == E_MonsterState.Die)
        {
            return;
        }
        e_MonsterState = E_MonsterState.Die;
        HitCollisionDetection.Instance.SetHit(this.gameObject, ScoreManager.E_ScoreState.Pass);
        Destroy(this.gameObject, 2f);
    }
}