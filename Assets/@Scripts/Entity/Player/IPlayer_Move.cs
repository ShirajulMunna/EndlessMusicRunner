using Unity.VisualScripting;
using UnityEngine;

public class IPlayer_Move : MonoBehaviour
{

    //현재 공격 위치
    [SerializeField] E_AttackPoint MovePoint = E_AttackPoint.Down;
    //내려가기 딜레이
    float CurDownDelay = 0.5f;
    const float MaxDownDelay = 0.5f;
    //움직임 속도
    const float MiddleMoveSpeed = 100;

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

    public void SetMove(E_AttackPoint point)
    {
        CurDownDelay -= Time.deltaTime;
        SetMovePoint(point);
        Move();
    }

    void SetMovePoint(E_AttackPoint point)
    {
        if (point == E_AttackPoint.None)
        {
            return;
        }
        MovePoint = point;
        CurDownDelay = 1;
    }

    //움직임 함수
    void Move()
    {
        if (CurDownDelay <= 0)
        {
            CurDownDelay = MaxDownDelay;
            MovePoint = E_AttackPoint.Down;
            player.SetAni(player.GetAniName(E_AniType.Running));
        }

        // 목표 위치 가져오기
        var targetPos = P_Attack.Tr_AttackVector[(int)MovePoint];
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
    public bool CheckHitActive(E_AttackPoint point)
    {
        if (MovePoint == E_AttackPoint.Middle)
        {
            return true;
        }

        return MovePoint == point;
    }

}