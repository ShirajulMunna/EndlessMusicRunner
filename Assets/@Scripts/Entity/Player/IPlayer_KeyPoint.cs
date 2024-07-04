using UnityEngine;

public class IPlayer_KeyPoint : MonoBehaviour
{
    IPlayer_Attack Attack
    {
        get => GameManager.instance.player.M_Attack;
    }

    // 위치 상태
    E_MovePoint MovePoint = E_MovePoint.None;
    public bool isHigt;


    bool isTwinAttack;
    float isHoldTime;
    bool isDKeyPressed;
    bool isFKeyPressed;
    bool isFKeyHandled;
    bool isJKeyHandled;


    public E_MovePoint SetPoint()
    {
        var point = !isHigt ? SetKey() : SetKey_Higt();

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
        if (Input.GetKeyDown(KeyCode.J))
        {
            isDKeyPressed = true;
            isFKeyHandled = true;
            CheckTwinKey();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            isFKeyPressed = true;
            isJKeyHandled = true;
            CheckTwinKey();
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            isDKeyPressed = false;
            isFKeyHandled = false;
            Reset();
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            isFKeyPressed = false;
            isJKeyHandled = false;
            Reset();
        }

        if (isFKeyHandled && isJKeyHandled)
        {
            isFKeyPressed = false;
            isDKeyPressed = false;
            isFKeyHandled = false;
            isJKeyHandled = false;
            return E_MovePoint.Up;
        }

        if (isDKeyPressed && !isFKeyPressed)
        {
            isDKeyPressed = false;
            return E_MovePoint.Up;
        }

        if (isFKeyPressed && !isDKeyPressed)
        {
            isFKeyPressed = false;
            return E_MovePoint.Down;
        }

        return E_MovePoint.None;
    }

    E_MovePoint SetKey_Higt()
    {
        // 동시 키 입력 처리
        if (Input.GetKeyDown(KeyCode.D))
        {
            isDKeyPressed = true;
            isFKeyHandled = true;
            CheckTwinKey();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isFKeyPressed = true;
            isJKeyHandled = true;
            CheckTwinKey();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            isDKeyPressed = false;
            isFKeyHandled = false;
            Reset();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            isFKeyPressed = false;
            isJKeyHandled = false;
            Reset();
        }

        if (isFKeyHandled && isJKeyHandled)
        {
            isFKeyPressed = false;
            isDKeyPressed = false;
            isFKeyHandled = false;
            isJKeyHandled = false;
            return E_MovePoint.Up;
        }

        if (isDKeyPressed && !isFKeyPressed)
        {
            isDKeyPressed = false;
            return E_MovePoint.Up;
        }

        if (isFKeyPressed && !isDKeyPressed)
        {
            isFKeyPressed = false;
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
