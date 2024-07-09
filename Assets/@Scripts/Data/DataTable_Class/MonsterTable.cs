using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterTable
{
    public int[] M_ID;
    public int[] Monster_Type;
    public int[] PrefabName;
    public int[] Uniq_MonsterType;
    public int[] MaxHp;
    public int[] Damage;
    public int[] Skill_1;
    public int[] Skill_2;
    public int[] Speed;


    public Dictionary<int, C_MonsterTable> Init()
    {
        Dictionary<int, C_MonsterTable> monsterTableDatas = new Dictionary<int, C_MonsterTable>();
        for (int i = 0; i < M_ID.Length; ++i)
        {
            var monster = new C_MonsterTable(PrefabName[i], Monster_Type[i], Uniq_MonsterType[i], MaxHp[i], Damage[i], Skill_1[i], Skill_2[i], Speed[i]);
            monsterTableDatas.Add(M_ID[i], monster);
        }

        return monsterTableDatas;
    }
}

[System.Serializable]
public class C_MonsterTable
{
    public int PrefabName;
    public int monsterType;
    public int Uniq_MonsterType;
    public int MaxHp;
    public int Damage;
    public int Skill_1;
    public int Skill_2;
    public int Speed;
    public C_MonsterTable(int PrefabName, int monsterType, int monstetType, int Hp,
        int Damage, int Skill1, int Skill2, int Speed)
    {
        this.PrefabName = PrefabName;
        this.monsterType = monsterType;
        Uniq_MonsterType = monstetType;
        MaxHp = Hp;
        this.Damage = Damage;
        Skill_1 = Skill1;
        Skill_2 = Skill2;
        this.Speed = Speed;
    }
}