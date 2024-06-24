using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelDesignMonsterCount
{
    private static Dictionary<int, int> stage_MonsterCounting = new Dictionary<int, int>();

    public static void Push(int id, int monsterCount)
    {
        if (stage_MonsterCounting.ContainsKey(id))
        {
            stage_MonsterCounting[id] += monsterCount;
        }
        else stage_MonsterCounting.Add(id, monsterCount);
    }
    public static int GetMonsterCount(int id)
    {
        return stage_MonsterCounting[id];
    }
}
[System.Serializable]
public class LevelDesign
{
    public int[] MID;
    public int[] MonsterInfo; // -> MonsterTable 코드로 변경해야함.
    public int[] MonsterSpwanCount;
    public float[] CoolTime;
    public int[] Spwan_Position;
    public float[] OffSetX, OffSetY;


    public Dictionary<int, List<C_LevelDesign>> Init()
    {
        Dictionary<int, List<C_LevelDesign>> datas = new Dictionary<int, List<C_LevelDesign>>();

        for (int i = 0; i < MID.Length; ++i)
        {
            var levelDesign = new C_LevelDesign(MonsterInfo[i], MonsterSpwanCount[i], CoolTime[i], Spwan_Position[i],OffSetX[i],OffSetY[i]);

            if (!datas.ContainsKey(MID[i]))
            {
                List<C_LevelDesign> C_LevelDesignList = new();
                C_LevelDesignList.Add(levelDesign);
                datas.Add(MID[i], C_LevelDesignList);
                LevelDesignMonsterCount.Push(MID[i], MonsterSpwanCount[i]);
            }
            else
            {
                datas[MID[i]].Add(levelDesign);
                LevelDesignMonsterCount.Push(MID[i], MonsterSpwanCount[i]);
            }
        }

        return datas;
    }
}
public class C_LevelDesign
{
    public int MonsterInfo;
    public int MonsterSpwanCount;
    public float CoolTime;
    public int Spwan_Position;
    public float OffSetX;
    public float OffSetY;

    public C_LevelDesign(int monsterInfo, int monsterSpwanCount, float coolTime, int spwan_Position, float OffSetX, float OffSetY)
    {
        MonsterInfo = monsterInfo;
        MonsterSpwanCount = monsterSpwanCount;
        CoolTime = coolTime;
        Spwan_Position = spwan_Position;
        this.OffSetX = OffSetX;
        this.OffSetY = OffSetY;
    }
}