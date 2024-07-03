public interface IMonsterAni
{
    int AttackCount { get; set; }
    int HitCount { get; set; }

    void SetHit();
    void SetAttack();
    void SetDie();
}