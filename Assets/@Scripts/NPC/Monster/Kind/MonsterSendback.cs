using UnityEngine;

public class MonsterSendback : Monsters
{
    float SpeedDelay;
    float MaxSpeedDelay = 0.15f;
    bool isHit;

    private void Update()
    {
        if (!isHit)
        {
            return;
        }

        SpeedDelay += Time.deltaTime;

        if (MaxSpeedDelay > SpeedDelay)
        {
            return;
        }
        SetRemove();
    }

    void SetRemove()
    {
        nPC_Move.SetSpeed(nPC_Status.GetSpeed());
        isHit = false;
        SpeedDelay = 0;
    }

    public override void SetHit(int hp)
    {
        base.SetHit(hp);
        nPC_Move.SetSpeed(0);
        isHit = true;
        SpeedDelay = 0;
    }
}