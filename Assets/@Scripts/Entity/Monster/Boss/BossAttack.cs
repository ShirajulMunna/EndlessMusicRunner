public interface IBossAttack
{
    E_BossAttack BossState { get; set; }
    int PlaySound { get; set; }
    BossCount bossCount { get; set; }
    void SetAni_Sound();
    void SetSound();
}

