using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class IPlayer_Particle
{
    public E_PlayerSkill SkillType;
    [SerializeField] GameObject[] G_Particle;

    [Header("해당 스킬 발동 시 꺼져야함")]
    [SerializeField] List<E_PlayerSkill> isOffList;
    float DelayTime;
    bool isActive;

    public void SetParticle(float activetime)
    {
        SetDirectActive(true);
        DelayTime = activetime;
    }

    public void SetActive()
    {
        if (!isActive)
        {
            return;
        }

        if (SetIdle())
        {
            return;
        }

        DelayTime -= Time.deltaTime;

        if (DelayTime > 0)
        {
            return;
        }
        SetDirectActive(false);
    }

    //꺼지지 않는 타입들
    public bool SetIdle()
    {
        return SkillType == E_PlayerSkill.Running || SkillType == E_PlayerSkill.Fly;
    }


    //강제로 온오프
    public void SetDirectActive(bool check)
    {
        foreach (var item in G_Particle)
        {
            item.SetActive(check);
        }
        isActive = check;
    }

    public void SetChange(E_PlayerSkill onskill)
    {
        if (isOffList.Count <= 0)
        {
            return;
        }

        var idx = isOffList.FindIndex(x => x == onskill);

        if (idx == -1)
        {
            return;
        }

        SetDirectActive(false);
    }
}
