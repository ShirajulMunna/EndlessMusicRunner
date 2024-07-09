using System.Threading.Tasks;
using UnityEngine;

public class Bosst : Monsters
{
    public static Bosst instance;
    public BossPattern_MoveAttack bossPattern_MoveAttack;

    //보스 위치
    Vector3 StartPos = new Vector3(13.5f, 0, 0);

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bossPattern_MoveAttack = GetComponent<BossPattern_MoveAttack>();
        PlayManager.instance.AddAction(E_Play.End, SetDie);
    }

    public Vector3 FirstPos()
    {
        return StartPos;
    }

    public override void SetActive()
    {
        base.SetActive();
        PlayManager.instance.SetAction(E_Play.Boss);
    }

    public override Vector3 SetMoveTarget()
    {
        return StartPos;
    }

    public override void SetHit(int hp)
    {
        base.SetHit(hp);
        CameraSystem.cameraSystem.SetZoomIn();
    }

    public override void SetDieMonster()
    {

    }
}