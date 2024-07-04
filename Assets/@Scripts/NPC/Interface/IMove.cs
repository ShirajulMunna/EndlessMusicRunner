using UnityEngine;

public interface IMove
{
    float Speed { get; set; }
    float arrivalThreshold { get; set; }
    bool isMoving { get; set; }
    Rigidbody2D rb { get; set; }
    Vector2 target { get; set; }
    E_MoveData e_MoveData { get; set; }
    void SetTarget(E_MoveData point);
    void SetTarget(Vector3 point);
    void MoveToTarget();
    void SetSpeed(float speed);

    //도달 했으면 false
    bool CheckIn();
    E_MoveData GetMoveData();
}