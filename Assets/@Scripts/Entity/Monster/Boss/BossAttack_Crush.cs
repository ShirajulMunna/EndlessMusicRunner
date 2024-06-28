using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossAttack_Crush : Monster, IBossAttack
{
    const string SoundName = "Boss_Attack_Sound_{0}";

    Boss boss
    {
        get
        {
            return Boss.instance;
        }
    }

    [Header("보스 공격")]
    [SerializeField] E_BossAttack _BossState;
    [SerializeField] int _PlaySound;

    public E_BossAttack BossState { get; set; }
    public int PlaySound { get; set; }
    public BossCount bossCount { get; set; } = new BossCount();

    float AniDelay = 1;
    int isPlayAni = 0;

    public override void SetUp(C_MonsterTable data, Vector3 cratepos)
    {
        base.SetUp(data, cratepos);
        PlaySound = _PlaySound;
        BossState = _BossState;
    }

    protected override void Update()
    {
        SetAni_Sound();
    }

    protected override void FixedUpdate()
    {

    }


    public void SetAni_Sound()
    {
        if (isPlayAni == 1)
        {
            AniDelay -= Time.deltaTime;

            if (AniDelay > 0)
            {
                return;
            }
            isPlayAni = 2;
            boss.SetAni(BossState);
            SetSound();
        }

        var check = bossCount.CheckPlay();

        if (!check)
        {
            return;
        }
        boss.SetAni(E_BossAttack.idle);
        isPlayAni = 1;
        boss.SetBossState(E_BossState.MoveAttack);
    }

    public void SetSound()
    {
        if (PlaySound <= 0)
        {
            return;
        }

        AudioManager.instance.PlayEffectSound(string.Format(SoundName, PlaySound));
    }
}
