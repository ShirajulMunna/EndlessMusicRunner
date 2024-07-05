using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Monster_Twin : Monster
{
    float delay = 0.1f;
    bool isDie;
    bool isSetDown;
    IPlayer_Move playerMove;

    const string Name = "Monster_{0}";

    public override void SetUp(C_MonsterTable data, Vector3 cratepos)
    {
        base.SetUp(data, cratepos);
        playerMove = GameManager.instance.GetPlayer(transform.position.y).M_Move;
    }

    protected override void Update()
    {
        base.Update();

        if (!isDie)
        {
            return;
        }

        delay -= Time.deltaTime;
        if (delay > 0)
        {
            return;
        }
        if (isSetDown)
        {
            return;
        }
        isSetDown = true;
        playerMove.SetDirrectMove(E_MovePoint.Down);
    }

    protected override void SetAttack(bool check)
    {
        if (!check)
        {
            return;
        }
        e_MonsterState = E_MonsterState.NoneAttack;
        SetComboReset();
        CreatPlayerHitEffect();
    }

    public override void SetMinusHp(int value)
    {
        base.SetMinusHp(value);
    }

    public override void SetHit()
    {
        base.SetHit();

    }

    public override void SetDie()
    {
        base.SetDie();
        playerMove.SetDirrectMove(E_MovePoint.Middle);
        isDie = true;
    }

    public override void SetHit(ScoreManager.E_ScoreState perfect)
    {
        base.SetHit(perfect);
        HitCollisionDetection.Instance.SetHit(this.gameObject, perfect);
    }
}
