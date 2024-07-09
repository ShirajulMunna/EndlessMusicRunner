using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class SkillSystem : Singleton<SkillSystem>
{
    public enum E_Trigger
    {
        None = 0,
        Combo = 1,
    }

    public enum E_SkillType
    {
        Active,
        Passive
    }

    System.Action Ac_CoolTime;
    Dictionary<KeyCode, System.Action> D_ActiveSkill = new Dictionary<KeyCode, System.Action>();
    Dictionary<int, KeyCode> D_ActiveskillKeys = new Dictionary<int, KeyCode>();

    Dictionary<E_Trigger, System.Action> D_PassiveSkill = new Dictionary<E_Trigger, System.Action>();

    Dictionary<int, SkillICON> D_SkillICON = new Dictionary<int, SkillICON>();

    public Transform Tr_SkillICONCreate;

    private void Start()
    {
        SetActiveSkill();
        SetPassiveSkill();
    }

    private void Update()
    {
        //쿨타임 돌리기 
        Ac_CoolTime?.Invoke();

        //버튼 눌렀나 확인 후 스킬 발동
        foreach (var key in D_ActiveSkill.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                D_ActiveSkill[key]?.Invoke();
            }
        }
    }

    //아이콘 생성
    async Task<bool> SetSkillICON(int skillid)
    {
        var check = D_SkillICON.ContainsKey(skillid);
        if (check)
        {
            return false;
        }

        var result = await SkillICON.Create(skillid);
        D_SkillICON.Add(skillid, result);
        return true;
    }

    //이미지 스킬 쿨타임 돌리기
    public void SetSkillICONCoolTime(int skillid, ISkillClass skillClass)
    {
        var check = D_SkillICON.TryGetValue(skillid, out var data);
        if (!check)
        {
            return;
        }
        data.SetCoolTime(skillClass);
    }

    //엑티브 스킬 셋팅
    async void SetActiveSkill()
    {
        //유저 스킬 리스트
        var skillidlist = new List<int>()
        {
            1,2,3,4
        };

        //유저 스킬 키 설정
        var userkeycodelist = new List<KeyCode>()
        {
            KeyCode.Q,
            KeyCode.W,
            KeyCode.E,
            KeyCode.R
        };

        int count = 0;

        //각 키별 스킬 추가
        for (int i = 0; i < skillidlist.Count; i++)
        {
            var skillid = skillidlist[i];

            //아이디를 이용해 사용 가능한 스킬들 가져오기
            var skilllist = GetActiveUserSkill(skillid);

            if (skilllist == null)
            {
                continue;
            }

            //키등록
            D_ActiveskillKeys.TryAdd(skillid, userkeycodelist[count]);
            var keycode = D_ActiveskillKeys[skillid];
            D_ActiveSkill.TryAdd(keycode, null);
            count++;

            // 키와 대응하는 동작 등록
            D_ActiveSkill[keycode] += skilllist;
        }

        foreach (var item in D_ActiveskillKeys)
        {
            await SetSkillICON(item.Key);
        }
    }




    //아이디를 이용해 스킬 리스트 가져오기
    public System.Action GetActiveUserSkill(int skillid)
    {
        var skilldata = skillid.GetSkillData();

        if (skilldata == null)
        {
            return null;
        }

        //패시브인지 체크 할 것
        if (skilldata.Type == 0)
        {
            return null;
        }

        System.Action skill = null;

        if (skilldata.st_AddScore.ActiveAddScore)
        {
            skill += () =>
            {
                SetScoreBooster(skilldata);
            };
        }

        if (skilldata.st_AddHeling.ActiveHealing)
        {
            skill += () =>
            {
                SetHealing(skilldata);
            };
        }

        if (skilldata.st_AddShield.ActiveShield)
        {
            skill += () =>
            {
                SetShield(skilldata);
            };
        }

        return skill;
    }

    //패시브 스킬 셋팅
    void SetPassiveSkill()
    {
        //유저 스킬 리스트
        var skillidlist = new List<int>()
        {
            1,2,3,4
        };

        foreach (var item in skillidlist)
        {
            //스킬 정보에서 트리거 가져오기
            var trigger = E_Trigger.Combo;

            var skill = GetPassiveUserSkill(item);
            if (skill == null)
            {
                continue;
            }

            D_PassiveSkill.TryAdd(trigger, null);
            D_PassiveSkill[trigger] += skill;

        }
    }

    //패시브 스킬 발동
    public void ActivePassiveSkill(E_Trigger trigger)
    {
        var check = D_PassiveSkill.TryGetValue(trigger, out var item);

        if (!check)
        {
            return;
        }

        item?.Invoke();
    }

    //패시브 스킬 가져오기
    public System.Action GetPassiveUserSkill(int skillid)
    {
        var skilldata = skillid.GetSkillData();

        if (skilldata == null)
        {
            return null;
        }

        //패시브인지 체크 할 것
        if (skilldata.Type == 1)
        {
            return null;
        }

        System.Action skill = null;

        if (skilldata.st_AddFever.ActiveFever)
        {
            skill += () =>
            {
                SetFeverTime(skilldata);
            };
        }

        return skill;
    }

    //쿨타임 적용
    public void SetCoolTime(System.Action action)
    {
        Ac_CoolTime += action;
    }

    //피버타임
    public void SetFeverTime(SkillData data)
    {
        FerverTimeSystem.Create(data);
    }

    //스코어부스터
    public void SetScoreBooster(SkillData data)
    {
        ScoreBooster.Create(data);
    }

    public void SetHealing(SkillData data)
    {
        Healing.Create(data, 100);
    }

    public void SetShield(SkillData data)
    {
        ShieldBuster.Create(data);
    }

    //스킬 키 변경 메서드
    public void ChangeSkillKey(int skillid, KeyCode newKey)
    {
        if (!D_ActiveskillKeys.ContainsKey(skillid))
        {
            Debug.LogError($"Skill name {skillid} does not exist.");
            return;
        }

        KeyCode oldKey = D_ActiveskillKeys[skillid];
        if (D_ActiveSkill.ContainsKey(oldKey))
        {
            System.Action action = D_ActiveSkill[oldKey];
            D_ActiveSkill.Remove(oldKey);
            D_ActiveSkill[newKey] = action;
        }

        D_ActiveskillKeys[skillid] = newKey;
    }
}