using System;
using UnityEngine;

public class NPC_Move : MonoBehaviour, IMove
{
    public Rigidbody2D rb { get; set; }
    public Vector2 target { get; set; } = IMovePoint.GetMovePoint(E_MoveData.Down);
    public bool isMoving { get; set; } = false;
    public float arrivalThreshold { get; set; } = 0.1f; // 목표 도달 판정 거리
    public float Speed { get => nPC_Status.Speed; set => Speed = value; }
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
        var dis = Vector3.Distance(rb.position, target);

        if (dis > arrivalThreshold)
        {
            var direction = (target - rb.position).normalized;
            rb.MovePosition(rb.position + direction * Speed * Time.fixedDeltaTime);
            return;
        }

        // 목표에 도달했으므로 이동 중지
        isMoving = false;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }
}