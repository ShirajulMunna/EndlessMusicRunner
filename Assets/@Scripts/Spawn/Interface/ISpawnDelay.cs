using System.Collections;

interface ISpawnDelay
{
    float PlaterAttackZone { get; set; }
    float MonsterSpeed { get; set; }
    IEnumerator IE_Delay(float delay, System.Action action);
    void SetDelay(float delay, System.Action action);
    float GetMonsterCreateDelay();
}