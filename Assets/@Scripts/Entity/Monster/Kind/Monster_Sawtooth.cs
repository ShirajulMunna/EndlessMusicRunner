using UnityEngine;

public class Monster_Sawtooth : Monster
{


    protected override void Update()
    {
        base.Update();
        SetScore();
    }


    public override void SetHit(ScoreManager.E_ScoreState perfect)
    {
        SetAttack(true);
    }

    protected override bool CheckAttack()
    {
        return false;
    }

    //스코어 획득
    void SetScore()
    {
        if (e_MonsterState == E_MonsterState.NoneAttack || e_MonsterState == E_MonsterState.Die)
        {
            return;
        }

        var targetpos = player.transform.position;

        if (targetpos.x < transform.position.x)
        {
            return;
        }
        SetDie();
        e_MonsterState = E_MonsterState.Die;
    }

    public override void SetDie()
    {
        HitCollisionDetection.Instance.SetHit(this.gameObject, ScoreManager.E_ScoreState.Pass);
        Destroy(this.gameObject,2f);
    }
}