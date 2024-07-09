using System.Threading.Tasks;
using UnityEngine;

public class FerverTimeSystem : Skill
{
    const string Name = "FeverTime_{0}";

    public static ISkillClass _skillClass;
    public static bool isActive;

    public static async void Create(SkillData st_Skill)
    {
        _skillClass = SkillClass.CreateClass(_skillClass);
        //이름 만들기
        var name = string.Format(Name, st_Skill.Objnum);

        if (!isActive)
        {
            UI_Play.instance.SetFever(st_Skill.Combo);
        }

        var result = await Skill.Create<FerverTimeSystem>(st_Skill, name, _skillClass);
        if (result == null)
        {
            return;
        }
        isActive = true;
        GameManager.M_Player.nPC_ParticleSystem.ActiveParticle(E_ParticleKind.Fever, st_Skill.Activetime);
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
        PlayManager.instance.AddAction(E_Play.Update, SetGage);
        PlayManager.instance.AddAction(E_Play.Update, SetCoolGage);
    }


    void SetGage()
    {
        CurremtTime -= Time.deltaTime;
        UI_Play.instance.SetMinusFever(ActiveTime, CurremtTime);
    }

    public void SetCoolGage()
    {
        if (isActive)
        {
            UI_Play.instance.SetFeverCoolTime(0);
            return;
        }

        var per = _skillClass.CoolTimeChecker.GetCoolTimePer();
        UI_Play.instance.SetFeverCoolTime(per);
        if (per < 1)
        {
            return;
        }
        PlayManager.instance.RemoveAction(E_Play.Update, SetCoolGage);
        UI_Play.instance.SetFeverCoolTime(0);
    }

    private void OnDestroy()
    {
        isActive = false;
        if (UI_Play.instance == null)
        {
            return;
        }
        PlayManager.instance.RemoveAction(E_Play.Update, SetGage);
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
