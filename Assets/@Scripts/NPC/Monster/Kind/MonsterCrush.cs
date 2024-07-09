using Unity.VisualScripting;
using UnityEngine;

public class MonsterCrush : Monsters
{
    [SerializeField] string AniName;

    enum E_State
    {
        Hit,
        CrushMove,
        Crush,
        Start,
        Move,
        Complted,
    }

    E_State e_State;
    float Delaytime = 0.3f;
    float Delaytime_Hit = 1f;
    System.Action Ac_Move_Complted;

    private void Start()
    {
        Bosst.instance.Ac_Hit += SetBossHit;
    }

    private void Update()
    {
        switch (e_State)
        {
            case E_State.Hit:
                UpdateHit();
                break;
            case E_State.Crush:
                UpdateCrush();
                break;
            case E_State.CrushMove:
            case E_State.Move:
                UpdateMove();
                break;
        }
    }

    public override void SetActive()
    {
        base.SetActive();
        SetState(E_State.CrushMove);
    }

    void UpdateHit()
    {
        Delaytime_Hit -= Time.deltaTime;

        if (Delaytime_Hit > 0)
        {
            return;
        }
        SetState(E_State.Start);
    }

    void UpdateMove()
    {
        if (Bosst.instance.nPC_Move.CheckIn())
        {
            return;
        }

        Ac_Move_Complted?.Invoke();
        Ac_Move_Complted = null;
    }

    void UpdateCrush()
    {
        Delaytime -= Time.deltaTime;

        if (Delaytime > 0)
        {
            return;
        }

        SetState(E_State.Start);
    }

    void SetBossHit()
    {
        SetState(E_State.Hit);
    }

    void SetState(E_State state)
    {
        switch (state)
        {
            case E_State.Hit:
                Bosst.instance.Ac_Hit -= SetBossHit;
                Bosst.instance.nPC_Move.SetSpeed(0);
                break;
            case E_State.CrushMove:
                Ac_Move_Complted += () => SetState(E_State.Crush);
                var pos = Bosst.instance.transform.position;
                pos.x = IMovePoint.GetMonster_Attack_Point_X();
                Bosst.instance.nPC_Move.SetTarget(pos);
                break;
            case E_State.Crush:
                Bosst.instance.Ac_Hit -= SetBossHit;
                Bosst.instance.iAni.SetAni(AniName, true);
                Bosst.instance.SetAttack(GameManager.M_Player);
                break;
            case E_State.Start:
                CameraSystem.cameraSystem.ReSetZoom();
                Ac_Move_Complted += () => SetState(E_State.Complted);
                Bosst.instance.nPC_Move.SetTarget(Bosst.instance.FirstPos());
                SetState(E_State.Move);
                break;
            case E_State.Move:
                Bosst.instance.iAni.SetAni(E_AniKind_Monster.Move, true);
                Bosst.instance.nPC_Move.SetSpeed(20);
                break;
            case E_State.Complted:
                Bosst.instance.iAni.SetAni(E_AniKind_Monster.idle, true);
                break;
        }
        e_State = state;
    }
}