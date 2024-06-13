using System.Threading.Tasks;
using UnityEngine;

public class FerverTimeSystem : Skill
{
    const string Name = "FeverTime_{0}";
    public static ISkillClass _skillClass;

    public static async void Create(SkillData st_Skill)
    {
        _skillClass = SkillClass.CreateClass(_skillClass);
        //이름 만들기
        var name = string.Format(Name, st_Skill.Objnum);

        var result = await Skill.Create<FerverTimeSystem>(st_Skill, name, _skillClass);
        if (result == null)
        {
            return;
        }
    }

    //피버타임 발동 시 스코어 두배
    public static int SetFeverScore(int currentScore)
    {
        if (_skillClass == null || _skillClass.ActiveChecker == null)
        {
            return currentScore;
        }

        return _skillClass.ActiveChecker.CheckActive() ? currentScore * 2 : currentScore;
    }
}

public struct St_AddFever
{
    public bool ActiveFever;

    public St_AddFever(bool ActiveFever)
    {
        this.ActiveFever = ActiveFever;
    }
}
