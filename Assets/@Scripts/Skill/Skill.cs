using System.Threading.Tasks;
using UnityEngine;

public class Skill : MonoBehaviour
{

    //생성 코드
    public static async Task<T> Create<T>(SkillData sk, string key, ISkillClass skillclass) where T : Object
    {
        if (!skillclass.ComboChecker.CheckComboCondition(sk.Combo) || !skillclass.CoolTimeChecker.CheckCoolTime())
        {
            return null;
        }
        var result = await key.CreateOBJ<Skill>();
        result.Setup(sk, skillclass);
        SkillSystem.instance.SetSkillICONCoolTime(sk.SkillID, skillclass);
        return result.GetComponent<T>();
    }

    private float _activeTime;

    protected ISkillClass skillClass;
    protected SkillData _SkillData;

    protected virtual void Update()
    {
        if (!SetActive())
        {
            return;
        }
    }

    //셋팅 함수
    public virtual void Setup(SkillData data, ISkillClass skillclass)
    {
        _SkillData = data;
        skillClass = skillclass;
        _activeTime = data.Activetime;
        skillClass.CoolTimeChecker.SetCoolTime(data.Cooltime);
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