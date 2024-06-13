using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] float Delay;
    [SerializeField] E_BossAttack BossState;
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
    }
}