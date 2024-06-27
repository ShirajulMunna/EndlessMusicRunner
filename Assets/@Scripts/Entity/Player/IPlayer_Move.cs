using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class IPlayer_Move : MonoBehaviour
{

    //현재 공격 위치
    [SerializeField] E_MovePoint MovePoint = E_MovePoint.Down;
    //내려가기 딜레이
    float CurDownDelay;
    const float MaxDownDelay = 0.2f;
    //움직임 속도
    const float MiddleMoveSpeed = 100;

    //이동 딜레이
    float ClearMoveDelay = 2f;

    PlayerSystem player
    {
        get => GameManager.instance.player;
    }
    IPlayer_Attack P_Attack
    {
        get => player.M_Attack;
    }

    Transform Tr
    {
        get => player.transform;
    }

    public E_MovePoint SetMove(E_MovePoint point)
    {
        CurDownDelay -= Time.deltaTime;
        SetMovePoint(point);
        Move();
        return MovePoint;
    }

    void SetMovePoint(E_MovePoint point)
    {
        if (MovePoint == E_MovePoint.Middle)
        {
            CurDownDelay = MaxDownDelay;
            return;
        }
        CurDownDelay = MaxDownDelay;
        if (point == E_MovePoint.None)
        {
            return;
        }
        MovePoint = point;
    }
    //움직임 함수
    void Move()
    {
        if (CurDownDelay <= 0)
        {
            CurDownDelay = MaxDownDelay;

            //플레이어의 위치에 따라서 하늘 또는 하단에 고정
            if (player.transform.position.y > 0)
            {
                MovePoint = E_MovePoint.Up;
                player.SetState(E_Entity_State.Fly);
            }
            else
            {
                MovePoint = E_MovePoint.Down;
                player.SetState(E_Entity_State.Running);
            }

        }
        // 목표 위치 가져오기
        var targetPos = P_Attack.Tr_AttackVector[GetMoveIDX(MovePoint)];
        var targetY = targetPos.y;
        var targetZ = targetPos.z;

        // 현재 위치 가져오기
        var currentPosition = Tr.position;

        // 새로운 위치 계산 (x는 고정)
        var newPos = new Vector3(currentPosition.x, Mathf.Lerp(currentPosition.y, targetY, Time.deltaTime * MiddleMoveSpeed), Mathf.Lerp(currentPosition.z, targetZ, Time.deltaTime * MiddleMoveSpeed));

        // 이동
        Tr.position = newPos;

        // 목표 위치에 거의 도달하면 루프 종료
        if (Mathf.Abs(currentPosition.y - targetY) < 0.01f && Mathf.Abs(currentPosition.z - targetZ) < 0.01f)
        {
            Tr.position = new Vector3(currentPosition.x, targetY, targetZ);
        }
    }

    //공격 가능 상태인지 및 위치 체크하여 공격 가능한 상태인지 체크
    public bool CheckHitActive(E_MovePoint point)
    {
        if (MovePoint == E_MovePoint.Middle)
        {
            return true;
        }

        return MovePoint == point;
    }

    //idx가져오기
    int GetMoveIDX(E_MovePoint point)
    {
        return (int)point;
    }

    //게임 종료 후 처리
    public void IsClearMove()
    {
        ClearMoveDelay -= Time.deltaTime;
        if (ClearMoveDelay > 0)
        {
            return;
        }
        Tr.transform.Translate(Vector3.right * Time.deltaTime * 15f);
    }


    //바로 이동
    public void DirectMove(E_MovePoint point)
    {
        var targetPos = P_Attack.Tr_AttackVector[GetMoveIDX(point)];
        Tr.position = targetPos;
    }

    //바로 이동
    public void SetDirrectMove(E_MovePoint point)
    {
        MovePoint = point;
        var targetPos = P_Attack.Tr_AttackVector[GetMoveIDX(point)];
        targetPos.x = Tr.position.x;
        Tr.position = targetPos;
    }
}