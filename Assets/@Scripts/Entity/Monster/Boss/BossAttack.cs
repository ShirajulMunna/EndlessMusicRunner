using UnityEngine;

public class BossAttack : MonoBehaviour
{
    const string SoundName = "Boss_Attack_Sound_{0}";

    [SerializeField] float Delay;
    [SerializeField] E_BossAttack BossState;
    [SerializeField] int PlaySound;
    bool CheckAttack;

    private void Update()
    {
        Delay -= Time.deltaTime;

        if (Delay > 0)
        {
            return;
        }

        if (CheckAttack)
        {
            return;
        }

        CheckAttack = true;
        Boss.instance.SetAni(BossState);
        SetSound();
    }


    void SetSound()
    {
        if (PlaySound <= 0)
        {
            return;
        }

        AudioManager.instance.PlayEffectSound(string.Format(SoundName, PlaySound));
    }
}