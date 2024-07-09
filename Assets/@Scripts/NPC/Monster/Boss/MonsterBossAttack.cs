using UnityEngine;

public class MonsterBossAttack : Monsters
{
    [SerializeField] string AniName;
    bool isCheck;
    bool isAttack;

    Player player
    {
        get => PlayerManager.instance.GetPlayer(transform.position.y);
    }


    private void Update()
    {
        UpdateMove();
    }

    public override void SetActive()
    {
        base.SetActive();
        try
        {
            isCheck = true;
            if (string.IsNullOrEmpty(AniName))
            {
                return;
            }
            Bosst.instance.iAni.SetAni(AniName, false);
        }
        catch
        {
            Debug.Log($"애니메이션 이름: {AniName}");
        }
    }

    void UpdateMove()
    {
        if (!isCheck)
        {
            return;
        }

        var ischeck = monsterAttack.CheckAttack();
        if (!ischeck)
        {
            return;
        }
        var pos = player.transform.position;
        player.player_Attacker.player_Effect.SetEffect(pos, ScoreManager.E_ScoreState.Pass);

        ScoreManager.instance.SetCurrentScore(1);
        isCheck = false;
        isAttack = true;
    }

    public override void SetDieMonster()
    {

    }

    public override void SetHit(int hp)
    {

    }

    public override void SetAttack(NPC target)
    {
        if (isAttack)
        {
            return;
        }

        base.SetAttack(target);
        isCheck = false;
        isAttack = true;
    }
}