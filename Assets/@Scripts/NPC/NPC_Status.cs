using UnityEngine;

public class NPC_Status : MonoBehaviour, IStatus
{
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int Damage { get; set; }
    public float Speed { get; set; }

    public void SetUp(int hp, float speed, int damage)
    {
        MaxHp = hp;
        CurrentHp = hp;
        Damage = damage;
        Speed = speed;
    }

    public void SetHp(int hp)
    {
        CurrentHp += hp;
    }

    public int GetDamage()
    {
        return Damage;
    }

    public bool CheckDie()
    {
        return CurrentHp <= 0;
    }

    public int GetHp()
    {
        return CurrentHp;
    }

    public float GetSpeed()
    {
        return Speed;
    }
}
