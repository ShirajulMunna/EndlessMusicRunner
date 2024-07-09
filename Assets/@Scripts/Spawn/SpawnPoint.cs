using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    const float Xvalue = 20;

    public Dictionary<E_MoveData, Vector3> D_MovePoint { get; set; } = IMovePoint.GetPoint();

    public Vector3 GetSpawnPoint(E_MoveData spwanPosition, float offsetx, float offsety)
    {
        var MySpwanPoint = GetPoint(spwanPosition);
        MySpwanPoint.x = Xvalue;
        
        MySpwanPoint.x += offsetx;
        MySpwanPoint.y += offsety;
        
        return MySpwanPoint;
    }

    //위치 가져오기
    public Vector3 GetPoint(E_MoveData spawnPoint)
    {
        return D_MovePoint[spawnPoint];
    }

}
