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

    public void SetMove(E_MovePoint point)
    {
        CurDownDelay -= Time.deltaTime;
        SetMovePoint(point);
        Move();
    }

    void SetMovePoint(E_MovePoint point)
    {
        if (point == E_MovePoint.None)
        {
            return;
        }
        MovePoint = point;
        CurDownDelay = MaxDownDelay;
    }

    //움직임 함수
    void Move()
    {
        //클리어시 플레이어 이동하는코드
        if (IsClearMove())
        {
            return;
        }

        if (CurDownDelay <= 0)
        {
            CurDownDelay = MaxDownDelay;
            MovePoint = E_MovePoint.Down;
            player.SetAni(player.GetAniName(E_AniType.Running));
            player.SetParticle(E_PlayerSkill.Running, 0);
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

    //플레이어 클리어시 이동 하는 함수 , 만약 플레이어가 죽으면 인동못하게 할예정
    bool isClearPlaying = false;
    public bool IsClearMove()
    {
        var isPlayerDie = player.CurHp <= 0;
        if (isPlayerDie) 
            return false;
        var isGameEnd = SpawnManager.instance.GetGameState() == E_GameState.End || 
            SpawnManager.instance.GetGameState() == E_GameState.GameOver;

        if (isGameEnd)
        {
            var results = FindObjectsOfType<Spine_GameResult>();
            if (results.Length > 0)
            {
                if(!isClearPlaying)
                {
                    isClearPlaying= true;
                    player.SetAni(player.GetAniName(E_AniType.Clear));
                }
                Tr.transform.Translate(Vector3.right * Time.deltaTime * 15f);
                return true;
            }
        }
        return false;
    }
}