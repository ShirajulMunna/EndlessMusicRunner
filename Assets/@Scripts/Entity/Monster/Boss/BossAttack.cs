using UnityEngine;

public class BossAttack : Monster
{
    const string SoundName = "Boss_Attack_Sound_{0}";

    [Space(10f)]
    [Header("보스 공격 관련ㅡㅡㅡㅡ")]

    [SerializeField] float Delay;
    [SerializeField] E_BossAttack BossState;
    [SerializeField] int PlaySound;

    [Header("생성위치 보스 지정한 곳에 생성")]
    [SerializeField] bool CustomCreatePos;
    [SerializeField] int CustomCreatePos_IDX;
    bool isCheckAniStart;
    Vector3 OriginPos;
    bool isDiagonalMoveComplete;

    protected override void Update()
    {
        base.Update();
        SetBossAni();
    }

    public override void SetUp(C_MonsterTable data, Vector3 cratepos)
    {
        base.SetUp(data, cratepos);
        SetCustomPos(cratepos);
    }

    //보스 공격 애니메이션 실행 
    void SetBossAni()
    {
        Delay -= Time.deltaTime;

        if (Delay > 0)
        {
            return;
        }

        if (isCheckAniStart)
        {
            return;
        }

        isCheckAniStart = true;
        Boss.instance.SetAni(BossState);
        SetSound();
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

    //커스텀 위치로 생성
    void SetCustomPos(Vector3 cratepos)
    {
        if (!CustomCreatePos)
        {
            return;
        }
        OriginPos = cratepos;

        var pos = Boss.instance.Tr_CustomCreate[CustomCreatePos_IDX].position;
        transform.position = pos;
    }

    public override void SetMove()
    {
        if (isDiagonalMoveComplete)
        {
            // y 값이 맞춰졌으면 x 방향으로만 이동
            base.SetMove();
        }
        else
        {
            // y 값 맞추기 위해 대각선으로 이동
            Vector3 targetPosition = new Vector3(transform.position.x, OriginPos.y, transform.position.z);
            float step = Speed * Time.deltaTime; // 이동 속도 설정

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    }

}