using UnityEngine;

public class MonsterBossAttack : Monsters
{
    [SerializeField] string AniName;

    public override void SetActive()
    {
        base.SetActive();
        try
        {
            if (string.IsNullOrEmpty(AniName))
            {
                return;
            }
            Bosst.instance.iAni.SetAni(AniName, false);
        }
        catch
        {
            Debug.LogError($"애니메이션 이름: {AniName}");
        }
    }

    public override void SetUp(int hp, float speed, int damage, Vector3 target)
    {
        base.SetUp(hp, speed, damage, target);
        GameLog.Log($"위치:{target}");
    }
}