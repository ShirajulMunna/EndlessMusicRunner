using UnityEngine;
using UnityEngine.PlayerLoop;

public class IPlayer_KeyPoint : MonoBehaviour
{
    PlayerSystem Player
    {
        get => GameManager.instance.player;
    }

    IPlayer_Attack Attack
    {
        get => GameManager.instance.player.M_Attack;
    }

    //위치 상태
    [SerializeField] E_MovePoint MovePoint = E_MovePoint.None;

    bool isTwinAttack;
    float isHoldTime;

    public E_MovePoint SetPoint()
    { 
        var point = SetKey();

        if (MovePoint == E_MovePoint.Middle)
        {
            return MovePoint;
        }

        if (point != E_MovePoint.None && !Attack.CheckHoldPoint() && MovePoint == point)
        {
            isHoldTime += Time.deltaTime;
        }

        if (isHoldTime > 0.3f)
        {
            return E_MovePoint.None;
        }

        MovePoint = point;
        return MovePoint;
    }

    E_MovePoint SetKey()
    {
        // 동시 키 입력 처리
        if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.J))
        {
            SetTwin(true);
            return E_MovePoint.Up;
        }

        if (Input.GetKey(KeyCode.F))
        {
            return E_MovePoint.Up;
        }

        if (Input.GetKey(KeyCode.J))
        {
            return E_MovePoint.Down;
        }

        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.J))
        {
            isHoldTime = 0;
            Attack.Reset();
        }

        return E_MovePoint.None;
    }

    //공격 위치 셋팅
    public void SetDirectMoveIdx(E_MovePoint idx)
    {
        MovePoint = idx;
    }

    //상태 체크
    public bool CheckPoint(E_MovePoint point)
    {
        return MovePoint == point;
    }

    //이중 입력 확인
    public bool CheckTwin()
    {
        return isTwinAttack;
    }

    //이중 입력 확인
    public void SetTwin(bool state)
    {
        isTwinAttack = state;
    }
}