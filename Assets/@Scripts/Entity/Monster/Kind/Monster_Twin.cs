using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class Monster_Twin : Monster
{
    IPlayer_KeyPoint player_State
    {
        get => GameManager.instance.player.M_State;
    }

    const string Name = "Monster_{0}";

    protected override void Start()
    {
        Ac_Hit += SetPlayerMiddleAttack;
        Ac_Die += PlayerAttackStateReset;
    }
    protected override void Update()
    {
        base.Update();
    }

    private void SetPlayerMiddleAttack()
    {
        player_State.SetDirectMoveIdx(E_MovePoint.Middle);
    }
    private void PlayerAttackStateReset()
    {
        player.M_Attack.Reset();
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
        player_State.SetDirectMoveIdx(E_MovePoint.Down);
    }
    public override void SetHit(ScoreManager.E_ScoreState perfect)
    {
        base.SetHit(perfect);
        HitCollisionDetection.Instance.SetHit(this.gameObject, perfect);
    }
}
