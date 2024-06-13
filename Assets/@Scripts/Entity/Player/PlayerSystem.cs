using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Spine.Unity;
using UnityEngine;

public class PlayerSystem : Entity
{
    public static PlayerSystem playerSystem;

    //공격 클래스
    public IPlayer_Attack M_Attack;
    //이동 클래스
    public IPlayer_Move M_Move;

    //애니메이션 리스트
    List<string> L_AniStr = new List<string>()
    {
        "Running",
        "fly",
        "Kick",
        "tail attack",
        "fire attack",
        "tail attack2",
        "retire",
    };

    private void Awake()
    {
        playerSystem = this;
        M_Attack = new IPlayer_Attack();
        M_Move = new IPlayer_Move();
    }

    private void Start()
    {
        var result = GetAniName(E_AniType.Running);
        SetAni(result);
        UI_Play.Instance.ActivatPanel(true);
    }

    private void Update()
    {
        var point = M_Attack.Attack();
        M_Move.SetMove(point);
    }

    //애니메이션 이름 가져오기
    public (string, bool) GetAniName(E_AniType state)
    {
        return (L_AniStr[(int)state], state == E_AniType.Running || state == E_AniType.Fly);
    }

    public override void SetHp(int value)
    {
        //hp감소일때 쉴드 체크
        if (value < 0)
        {
            var checkshield = ShieldBuster.CheckShield();
            if (checkshield)
            {
                return;
            }

        }
        base.SetHp(value);
        UI_Play.Instance.SetHp(MaxHp, CurHp);
    }

    public override void SetMinusHp(int value)
    {
        base.SetMinusHp(value);

        //콤보 리셋 및 MISS추가
        ScoreManager.instance.SetScoreState(ScoreManager.E_ScoreState.Miss);
        ScoreManager.instance.SetBestCombo_Reset();

        //공격 사운드 및 애니메이션 처리
        SetAni(GetAniName(E_AniType.Hit));
        AudioManager.instance.PlayerHItSound();
    }

    public override void SetDie()
    {
        base.SetDie();

        //사망 후 처리
        AudioManager.instance.StopMusic();
        SpawnManager.instance.StopAllCoroutines();
        UI_GameOver.Create();
    }
}