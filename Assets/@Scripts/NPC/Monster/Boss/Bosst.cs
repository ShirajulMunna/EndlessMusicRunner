using System.Threading.Tasks;
using UnityEngine;

public class Bosst : Monsters
{
    public static Bosst instance;
    public BossPattern_MoveAttack bossPattern_MoveAttack;

    public System.Action Ac_Hit;
    //보스 위치
    Vector3 StartPos = new Vector3(13.5f, 0, 0);

    System.Action Ac_Die;
    float DieDleay = 1f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bossPattern_MoveAttack = GetComponent<BossPattern_MoveAttack>();

        ActionManager.instance.AddAction((E_ActionScene.Play, E_ActionList.End), () =>
        {
            if (PlayerManager.instance.GetPlayer(0).nPC_Status.CheckDie())
            {
                return;
            }
            Ac_Die = UpdateDie;
        });
    }

    private void Update()
    {
        Ac_Die?.Invoke();
    }

    void UpdateDie()
    {
        DieDleay -= Time.deltaTime;
        if (DieDleay > 0)
        {
            return;
        }
        Ac_Die = null;

        SetDie();
    }

    public Vector3 FirstPos()
    {
        return StartPos;
    }

    public override void SetActive()
    {
        base.SetActive();
        ActionManager.instance.SetAction((E_ActionScene.Play, E_ActionList.Boss));
    }

    public override Vector3 SetMoveTarget()
    {
        return StartPos;
    }

    public override void SetHit(int hp)
    {
        base.SetHit(hp);
        Ac_Hit?.Invoke();
    }

    public override void SetDieMonster()
    {

    }
    public override void SetDie()
    {
        iAni?.SetAni(E_AniKind_Monster.Die, false, true);
    }

    public override void SetAttack(NPC target)
    {
        base.SetAttack(target);
        GameLog.Log("보스 공격!");
    }
}