using System.Threading.Tasks;
using UnityEngine;

public class FerverTimeSystem : Skill
{
    const string Name = "FeverTime_{0}";
    const string SkyName = "FeverTime_Sky_{0}";
    const string BackName = "FeverTime_Back_{0}";

    public static ISkillClass _skillClass;
    public static SkillData skillData;
    public static bool isActive;

    public static async void Create(SkillData st_Skill)
    {
        _skillClass = SkillClass.CreateClass(_skillClass);
        //이름 만들기
        var name = string.Format(Name, st_Skill.Objnum);

        if (!isActive)
        {
            UI_Play.Instance.SetFever(st_Skill.Combo);
        }

        var result = await Skill.Create<FerverTimeSystem>(st_Skill, name, _skillClass);
        if (result == null)
        {
            return;
        }
        isActive = true;
        GameManager.instance.player.SetParticle(E_PlayerSkill.Fever, st_Skill.Activetime);
        AudioManager.instance.PlayEffectSound("Fever_Time");
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

    float ActiveTime;
    float CurremtTime;

    public override void Setup(SkillData data, ISkillClass skillclass)
    {
        base.Setup(data, skillclass);
        ActiveTime = data.Activetime;
        CurremtTime = ActiveTime;
        UI_Play.Instance.Ac_Update += SetGage;
        UI_Play.Instance.Ac_Update += SetCoolGage;
    }


    void SetGage()
    {
        CurremtTime -= Time.deltaTime;
        UI_Play.Instance.SetMinusFever(ActiveTime, CurremtTime);
    }

    public void SetCoolGage()
    {
        if (isActive)
        {
            UI_Play.Instance.SetFeverCoolTime(0);
            return;
        }

        var per = _skillClass.CoolTimeChecker.GetCoolTimePer();
        print(per);
        UI_Play.Instance.SetFeverCoolTime(per);
        if (per > 0)
        {
            return;
        }
        UI_Play.Instance.Ac_Update -= SetCoolGage;
        UI_Play.Instance.SetFeverCoolTime(0);
    }

    private void OnDestroy()
    {
        if (UI_Play.Instance == null)
        {
            return;
        }

        isActive = false;
        UI_Play.Instance.Ac_Update -= SetGage;
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
