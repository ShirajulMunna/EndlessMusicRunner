using UnityEngine;

public class ShieldBuster : Skill
{
    const string Name = "ShieldBuster_{0}";
    public static ISkillClass _skillClass;

    public static async void Create(SkillData st_Skill)
    {
        _skillClass = SkillClass.CreateClass(_skillClass);
        var name = string.Format(Name, st_Skill.Objnum);
        var result = await Skill.Create<ShieldBuster>(st_Skill, name, _skillClass);
        if (result == null)
        {
            return;
        }
        GameManager.instance.player.SetParticle(E_PlayerSkill.Shield, st_Skill.Activetime);
    }

    //쉴드 작동 체크
    public static bool CheckShield()
    {
        if (_skillClass == null || _skillClass.ActiveChecker == null)
        {
            return false;
        }
        return _skillClass.ActiveChecker.CheckActive();
    }
}

public struct st_AddShield
{
    public bool ActiveShield;

    public st_AddShield(bool ActiveShield)
    {
        this.ActiveShield = ActiveShield;
    }
}
