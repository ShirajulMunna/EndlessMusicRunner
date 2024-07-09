using System.Collections.Generic;
using UnityEngine;

public static class IMovePoint
{
    static Dictionary<E_MoveData, Vector3> D_MovePoint = new Dictionary<E_MoveData, Vector3>()
    {
        { E_MoveData.Higt_Higt, new Vector3(-15,5f,0)},
        { E_MoveData.Higt_Middle, new Vector3(-15,3.5f,0)},
        { E_MoveData.Higt_Twin, new Vector3(-15,3.5f,0)},
        { E_MoveData.Higt_Low, new Vector3(-15,2f)},
        { E_MoveData.Low_Higt, new Vector3(-15,-1f)},
        { E_MoveData.Low_Middle, new Vector3(-15,-2.5f)},
        { E_MoveData.Low_Low, new Vector3(-15,-4f)},
        { E_MoveData.Low_Twin, new Vector3(-15,-2.5f)},
    };

    static float Monster_Max_Move_X_Value = -40f;

    /// <summary>
    /// 몬스터 최대 이동 거리
    /// </summary>
    public static float GetMax_X_value()
    {
        return Monster_Max_Move_X_Value;
    }

    static float Monster_Attack_Point_X = -14f;

    /// <summary>
    /// 몬스터 최대 이동 거리
    /// </summary>
    public static float GetMonster_Attack_Point_X()
    {
        return Monster_Attack_Point_X;
    }

    public static Vector3 GetMovePoint(E_MoveData e_MoveData)
    {
        return D_MovePoint[e_MoveData];
    }

    public static Dictionary<E_MoveData, Vector3> GetPoint()
    {
        return D_MovePoint;
    }

    public static bool CheckMiddle_Twin(E_MoveData targetdata, E_MoveData e_MoveData)
    {
        var hight = targetdata == E_MoveData.Higt_Higt || targetdata == E_MoveData.Higt_Low;
        if (hight)
        {
            var higtCheck = e_MoveData == E_MoveData.Higt_Middle || e_MoveData == E_MoveData.Higt_Twin;
            return higtCheck;
        }

        var low = targetdata == E_MoveData.Low_Higt || targetdata == E_MoveData.Low_Low;
        var lowCheck = low && (e_MoveData == E_MoveData.Low_Middle || e_MoveData == E_MoveData.Low_Twin);

        return lowCheck;
    }
}

