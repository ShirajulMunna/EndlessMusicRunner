using System.Threading.Tasks;
using UnityEngine;

public class Skill : MonoBehaviour
{
    //생성 코드
    public static async Task<T> Create<T>(SkillData sk, string key, ISkillClass skillclass) where T : Object
    {
        skillclass.SetClass();
        if(skillclass.CoolTimeChecker.CheckCoolTime())
        {
            UI_Play.Instance.Img_Fever.fillAmount = (float)ScoreManager.instance.CurrentCombo / (float)sk.Combo;
        }
        if (!skillclass.ComboChecker.CheckComboCondition(sk.Combo) || !skillclass.CoolTimeChecker.CheckCoolTime())
        {
            return null;
        }
        var result = await key.CreateOBJ<Skill>();
        result.Setup(sk.Activetime, sk.Cooltime, skillclass);
        SkillSystem.instance.SetSkillICONCoolTime(sk.SkillID, skillclass);
        return result.GetComponent<T>();
    }

    private float _activeTime;

    protected ISkillClass skillClass;

    protected virtual void Update()
    {
        if (!SetActive())
        {
            return;
        }
    }

    //셋팅 함수
    public virtual void Setup(float activetime, float cooltime, ISkillClass skillclass)
    {
        skillClass = skillclass;
        _activeTime = activetime;
        skillClass.CoolTimeChecker.SetCoolTime(cooltime);
        skillClass.ActiveChecker.SetActive(true);
    }

    //지속시간 확인
    protected virtual bool SetActive()
    {
        _activeTime -= Time.deltaTime;
        if (_activeTime <= 0)
        {
            Destroy(this.gameObject);
            return false;
        }
        return true;
    }

    private void OnDestroy()
    {
        skillClass.ActiveChecker.SetActive(false);
    }
}