using System;
using UnityEngine;

public class NPC_Move : MonoBehaviour, IMove
{
    public Rigidbody2D rb { get; set; }
    public Vector2 target { get; set; } = IMovePoint.GetMovePoint(E_MoveData.Down);
    public bool isMoving { get; set; } = false;
    public float arrivalThreshold { get; set; } = 0.1f; // 목표 도달 판정 거리
    public float Speed { get => nPC_Status.GetSpeed(); set => nPC_Status.Speed = value; }
    public E_MoveData e_MoveData { get; set; }

    NPC_Status _nPC_Status;
    NPC_Status nPC_Status
    {
        get
        {
            if (_nPC_Status == null)
            {
                _nPC_Status = GetComponent<NPC_Status>();
            }

            return _nPC_Status;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isMoving)
        {
            return;
        }
        MoveToTarget();
    }

    public void SetTarget(E_MoveData point)
    {
        if (GetMoveData() == point)
        {
            return;
        }

        target = IMovePoint.GetMovePoint(point);
        e_MoveData = point;
        isMoving = true;
    }

    public void SetTarget(Vector3 point)
    {
        target = point;

        foreach (E_MoveData moveData in Enum.GetValues(typeof(E_MoveData)))
        {
            var points = IMovePoint.GetMovePoint(moveData);
            if (points.y == point.y)
            {
                e_MoveData = moveData;
                break;
            }
        }
        isMoving = true;
    }

    public E_MoveData GetMoveData()
    {
        return e_MoveData;
    }

    public void MoveToTarget()
    {
        if (CheckIn())
        {
            var direction = (target - rb.position).normalized;
            var newPosition = rb.position + direction * Speed * Time.fixedDeltaTime;

            // 목표 지점을 지나치지 않도록 보정
            newPosition.x = Mathf.Clamp(newPosition.x, Mathf.Min(rb.position.x, target.x), Mathf.Max(rb.position.x, target.x));
            newPosition.y = Mathf.Clamp(newPosition.y, Mathf.Min(rb.position.y, target.y), Mathf.Max(rb.position.y, target.y));

            rb.MovePosition(newPosition);
            return;
        }

        // 목표에 도달했으므로 정확한 위치로 설정하고 이동 중지
        rb.position = target;
        isMoving = false;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public bool CheckIn()
    {
        var dx = Mathf.Abs(rb.position.x - target.x);
        var dy = Mathf.Abs(rb.position.y - target.y);
        return dx > arrivalThreshold || dy > arrivalThreshold;
    }
}