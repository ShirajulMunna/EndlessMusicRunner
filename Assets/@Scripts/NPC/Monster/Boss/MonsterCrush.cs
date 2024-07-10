using Unity.VisualScripting;
using UnityEngine;

public class MonsterCrush : Monsters
{
    [SerializeField] string AniName;

    enum E_State
    {
        idle,
        Hit,
        CrushMove,
        Crush,
        Start,
        Move,
        Complted,
    }

    E_State e_State = E_State.idle;
    float Delaytime = 0.5f;
    float Delaytime_Hit = 1f;
    System.Action Ac_Move_Complted;

    Bosst bosst
    {
        get => Bosst.instance;
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
        bosst.Ac_Hit += SetBossHit;
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
        if (bosst.nPC_Move.CheckIn())
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
        CameraSystem.instance.SetZoomIn();
    }

    void SetState(E_State state)
    {
        switch (state)
        {
            case E_State.Hit:
                bosst.Ac_Hit = null;
                bosst.nPC_Move.SetSpeed(0);
                break;
            case E_State.CrushMove:
                Ac_Move_Complted += () => SetState(E_State.Crush);
                var pos = bosst.transform.position;
                pos.x = IMovePoint.GetMonster_Attack_Point_X();
                bosst.nPC_Move.SetTarget(pos);
                break;
            case E_State.Crush:
                bosst.Ac_Hit = null;
                bosst.iAni.SetAni(AniName, true);
                bosst.SetAttack(PlayerManager.instance.GetPlayer(bosst.transform.position.y));
                break;
            case E_State.Start:
                CameraSystem.instance.ReSetZoom();
                Ac_Move_Complted += () => SetState(E_State.Complted);
                bosst.nPC_Move.SetTarget(bosst.FirstPos());
                SetState(E_State.Move);
                state = E_State.Move;
                break;
            case E_State.Move:
                bosst.iAni.SetAni(E_AniKind_Monster.Move, true);
                bosst.nPC_Move.SetSpeed(20);
                break;
            case E_State.Complted:
                bosst.iAni.SetAni(E_AniKind_Monster.idle, true);
                break;
        }
        e_State = state;
    }
}