using Unity.VisualScripting;

public class Monster_Twin : Monster
{
    IPlayer_State player_State
    {
        get => GameManager.instance.player.M_State;
    }

    const string Name = "Monster_{0}";

    protected override void Update()
    {
        base.Update();
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

    public override void SetHit(ScoreManager.E_ScoreState perfect)
    {
        base.SetHit(perfect);
    }
}
