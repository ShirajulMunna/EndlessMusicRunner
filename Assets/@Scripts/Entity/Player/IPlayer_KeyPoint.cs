using UnityEngine;

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

    // 위치 상태
    [SerializeField] E_MovePoint MovePoint = E_MovePoint.None;

    bool isTwinAttack;
    float isHoldTime;
    bool isFKeyPressed;
    bool isJKeyPressed;
    bool isFKeyHandled;
    bool isJKeyHandled;

    public E_MovePoint SetPoint()
    {
        var point = SetKey();

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFKeyPressed = true;
            isFKeyHandled = true;
            CheckTwinKey();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            isJKeyPressed = true;
            isJKeyHandled = true;
            CheckTwinKey();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            isFKeyPressed = false;
            isFKeyHandled = false;
            Reset();
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            isJKeyPressed = false;
            isJKeyHandled = false;
            Reset();
        }

        if (isFKeyHandled && isJKeyHandled)
        {
            isJKeyPressed = false;
            isFKeyPressed = false;
            isFKeyHandled = false;
            isJKeyHandled = false;
            return E_MovePoint.Up;
        }

        if (isFKeyPressed && !isJKeyPressed)
        {
            isFKeyPressed = false;
            return E_MovePoint.Up;
        }

        if (isJKeyPressed && !isFKeyPressed)
        {
            isJKeyPressed = false;
            return E_MovePoint.Down;
        }

        return E_MovePoint.None;
    }

    void CheckTwinKey()
    {
        if (isFKeyHandled && isJKeyHandled)
        {
            SetTwin(true);
        }
    }

    void Reset()
    {
        isHoldTime = 0;
        Attack.Reset();
    }

    // 공격 위치 셋팅
    public void SetDirectMoveIdx(E_MovePoint idx)
    {
        MovePoint = idx;
    }

    // 이중 입력 확인
    public bool CheckTwin()
    {
        return isTwinAttack;
    }

    // 이중 입력 설정
    public void SetTwin(bool state)
    {
        isTwinAttack = state;
    }
}
