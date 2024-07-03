using System.Collections.Generic;
using UnityEngine;

public static class IMovePoint
{
    static Dictionary<E_MoveData, Vector3> D_MovePoint = new Dictionary<E_MoveData, Vector3>()
    {
        { E_MoveData.Down, new Vector3(-15,-3.5f,0)},
        { E_MoveData.Middle, new Vector3(-15,0,0)},
        { E_MoveData.Up, new Vector3(-15,3.5f)},
        { E_MoveData.Twin, new Vector3(-15,3.5f)},
    };

    static float Monster_Max_Move_X_Value = -25f;

    /// <summary>
    /// 몬스터 최대 이동 거리
    /// </summary>
    public static float GetMax_X_value()
    {
        return Monster_Max_Move_X_Value;
    }

    public static Vector3 GetMovePoint(E_MoveData e_MoveData)
    {
        return D_MovePoint[e_MoveData];
    }
}

