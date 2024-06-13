using UnityEngine;

public class Healing : Skill
{
    const string Name = "Healing_{0}";
    public static ISkillClass _skillClass;

    public static async void Create(SkillData st_Skill, int healing)
    {
        _skillClass = SkillClass.CreateClass(_skillClass);
        var name = string.Format(Name, st_Skill.Objnum);
        var result = await Skill.Create<Healing>(st_Skill, name, _skillClass);
        if (result == null)
        {
            return;
        }
        result?.SetHealing(healing);
    }

    public void SetHealing(int healing)
    {
        GameManager.instance.player.SetHp(healing);
    }
}

public struct St_AddHeling
{
    public bool ActiveHealing { get; private set; }
    public int Healing { get; private set; }

    public St_AddHeling(bool ActiveHealing, int Healing)
    {
        this.ActiveHealing = ActiveHealing;
        this.Healing = Healing;
    }
}
