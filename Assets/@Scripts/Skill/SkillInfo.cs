using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public static class SkillInfo
{
    const string ICONName = "SkillICON_{0}";
    static Dictionary<int, SkillData> D_SkillData = new Dictionary<int, SkillData>()
    {
        {4, new SkillData(4,0,10,10,30,0,default,default,new St_AddFever(true),default)},
        {1, new SkillData(1,1,0,10,10,0,default,new st_AddShield(true),default,default)},
        {2, new SkillData(2,1,0,1,5,0,default,default,default,new St_AddHeling(true,100))},
        {3, new SkillData(3,1,0,1,5,0,new St_AddScore(true, 1000),default,default,default)},
    };

    //스킬 아이콘 가져오기
    public static async Task<Sprite> GetSkillICON(this int skillid)
    {
        //스킬 정보에서 아이콘 정보 가져오기
        var iconid = skillid;
        var key = string.Format(ICONName, iconid);
        var result = await AddressLoad.LoadAsync<Sprite>(key);

        return result;
    }

    //스킬 정보 불러오기
    public static SkillData GetSkillData(this int id)
    {
        var check = D_SkillData.TryGetValue(id, out var data);

        if (!check)
        {
            return null;
        }

        return data;
    }
}



public class SkillData
{
    public int SkillID { get; private set; }
    public int Type { get; private set; }
    public int Combo { get; private set; }
    public float Activetime { get; private set; }
    public float Cooltime { get; private set; }
    public int Objnum { get; private set; }

    public St_AddScore st_AddScore;

    public st_AddShield st_AddShield;

    public St_AddFever st_AddFever;

    public St_AddHeling st_AddHeling;

    public SkillData(int skillID, int type, int combo, float activetime, float cooltime, int objnum, St_AddScore st_AddScore, st_AddShield st_AddShield, St_AddFever st_ScoreBooster, St_AddHeling st_AddHeling)
    {
        SkillID = skillID;
        Type = type;
        Combo = combo;
        Activetime = activetime;
        Cooltime = cooltime;
        Objnum = objnum;
        this.st_AddScore = st_AddScore;
        this.st_AddShield = st_AddShield;
        this.st_AddFever = st_ScoreBooster;
        this.st_AddHeling = st_AddHeling;
    }
}
