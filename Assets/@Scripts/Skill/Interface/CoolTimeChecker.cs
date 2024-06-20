using UnityEngine;

public interface ICoolTimeChecker
{
    bool CheckCoolTime();
    void SetCoolTime(float cooltime);
    void UpdateCoolTime();

    float GetCurrentCoolTime();
    float GetMaxCoolTime();

    void SetCoolTimeData();
}


public class CoolTimeChecker : ICoolTimeChecker
{
    public float _MaxCoolTime;
    public float CoolTime;

    public bool CheckCoolTime()
    {
        return CoolTime <= 0;
    }

    public float GetCurrentCoolTime()
    {
        return CoolTime;
    }

    public float GetMaxCoolTime()
    {
        return _MaxCoolTime;
    }

    public void SetCoolTime(float cooltime)
    {
        _MaxCoolTime = cooltime;
        CoolTime = cooltime;
    }

    public void UpdateCoolTime()
    {
        CoolTime -= Time.deltaTime;
    }

    public void SetCoolTimeData()
    {
        SkillSystem.instance.SetCoolTime(UpdateCoolTime);
    }
}