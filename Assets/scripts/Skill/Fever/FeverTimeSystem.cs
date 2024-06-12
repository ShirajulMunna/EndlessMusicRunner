using UnityEngine;

public class FerverTimeSystem : Skill
{
    const string Name = "FeverTime_{0}";
    public static ISkillClass _skillClass;

    public static async void Create(St_Skill st_Skill)
    {
        _skillClass = SkillClass.CreateClass(_skillClass);
        var name = string.Format(Name, st_Skill.objnum);
        await Skill.Create<FerverTimeSystem>(st_Skill, name, _skillClass);
    }

    public static int SetFeverScore(int currentScore)
    {
        return _skillClass.ActiveChecker.CheckActive() ? currentScore * 2 : currentScore;
    }
}