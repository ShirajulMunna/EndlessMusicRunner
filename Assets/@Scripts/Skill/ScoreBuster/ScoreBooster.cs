using TMPro;
using UnityEngine;

public class ScoreBooster : Skill
{
    const string Name = "ScoreBootser_{0}";
    public static ISkillClass _skillClass;
    public static async void Create(SkillData st_Skill)
    {
        _skillClass = SkillClass.CreateClass(_skillClass);
        var name = string.Format(Name, st_Skill.Objnum);
        var result = await Skill.Create<ScoreBooster>(st_Skill, name, _skillClass);
        if (result == null)
        {
            return;
        }
        result?.SetAddScore(st_Skill.st_AddScore.Addscore);
        GameManager.instance.player.SetParticle(E_PlayerSkill.ScoreBooster, st_Skill.Activetime);
    }

    [SerializeField] TextMeshProUGUI T_AddScore;

    //스코어 추가
    public void SetAddScore(int addscore)
    {
        T_AddScore.text = "+" + addscore;
        ScoreManager.instance.SetCurrentScore(addscore, true);
    }
}

public struct St_AddScore
{
    public bool ActiveAddScore;
    public int Addscore;

    public St_AddScore(bool ActiveAddScore, int addscore)
    {
        this.ActiveAddScore = ActiveAddScore;
        Addscore = addscore;
    }
}
