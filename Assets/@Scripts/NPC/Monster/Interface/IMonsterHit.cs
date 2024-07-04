interface IMonsterHit
{
    IMove nPC_Move { get; set; }
    Monsters MonsTer { get; set; }
    float DelayTime { get; set; }
    void UpdateState_Hit();
    void SetState_Hit();
}