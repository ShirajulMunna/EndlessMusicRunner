using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Monster_Twin : Monster
{
    IPlayer_Move playerMove
    {
        get => GameManager.instance.player.M_Move;
    }

    const string Name = "Monster_{0}";

    protected override void Start()
    {
        Ac_Hit += SetPlayerMiddleAttack;
        Ac_Die += SetPlayerMiddleAttack;
    }
    protected override void Update()
    {
        base.Update();
    }

    private void SetPlayerMiddleAttack()
    {
        playerMove.SetDirrectMove(E_MovePoint.Middle);
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

    public override void SetDie()
    {
        base.SetDie();
    }
    private void OnDestroy()
    {
        playerMove.SetDirrectMove(E_MovePoint.Down);
    }
    public override void SetHit(ScoreManager.E_ScoreState perfect)
    {
        base.SetHit(perfect);
        HitCollisionDetection.Instance.SetHit(this.gameObject, perfect);
    }
}
