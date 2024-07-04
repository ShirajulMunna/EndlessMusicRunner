using UnityEngine;

public interface IMonster
{
    System.Action Ac_Active { get; set; }
    IMonsterType monsterType { get; set; }
    IMonsterAttack monsterAttack { get; set; }
    IMonsterAni iAni { get; set; }
    NPC npc { get; set; }
    MonsterState monsterState { get; set; }
    //생성 처리 함수
    void CreateMonster(C_MonsterTable data, Vector3 cratepos);

    //몬스터 온 처리
    void SetActive();

    //사망 처리
    void SetDieMonster();
}