using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class IPlayer_Particle
{
    public E_PlayerSkill SkillType;
    [SerializeField] GameObject[] G_Particle;
    float DelayTime;
    bool isActive;

    public void SetParticle(float activetime)
    {
        foreach (var item in G_Particle)
        {
            item.SetActive(true);
        }

        DelayTime = activetime;
        isActive = true;
    }

    public void SetActive()
    {
        if (!isActive)
        {
            return;
        }
        DelayTime -= Time.deltaTime;

        if (DelayTime > 0)
        {
            return;
        }
        foreach (var item in G_Particle)
        {
            item.SetActive(false);
        }
        isActive = false;
    }
}
