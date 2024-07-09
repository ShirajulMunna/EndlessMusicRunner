using UnityEngine;

public class MonsterTypeData : MonoBehaviour, IMonsterType
{
    public E_MonsterType e_MonsterType { get; set; }

    public E_MonsterType GetMonsterType()
    {
        return e_MonsterType;
    }

    public void SetMonsterType(E_MonsterType e_monsterTypes)
    {
        e_MonsterType = e_monsterTypes;
    }
}