using UnityEngine;

public class MonsterBossAttack : Monsters
{
    [SerializeField] string AniName;
    bool isCheck;
    bool isAttack;


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

        var x = transform.position.x;
        var targetx = GameManager.M_Player.transform.position.x - 1;
        if (x > targetx)
        {
            return;
        }

        ScoreManager.instance.SetCurrentScore(1);
        var pos = GameManager.M_Player.transform.position;
        GameManager.M_Player.player_Attacker.player_Effect.SetEffect(pos, ScoreManager.E_ScoreState.Pass);
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
    }


    public override void SetDie()
    {
        base.SetDie();
    }
}