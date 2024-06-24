using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    //스폰 위치
    public List<Vector3> L_SpawnPoint { get; set; } = new List<Vector3>()
    {
        new Vector3(20, -3.5f, 0),
        new Vector3(20, 0, 0),
        new Vector3(20, 3.5f, 0),
    };

    public Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition, float offsetx, float offsety)
    {
        var MySpwanPoint = GetPoint(E_SpawnPoint.Hight);

        switch (spwanPosition)
        {
            case MonsterSpwanPosition.Down:
                MySpwanPoint = GetPoint(E_SpawnPoint.Low);
                break;
            case MonsterSpwanPosition.Middle:
                MySpwanPoint = GetPoint(E_SpawnPoint.Middle);
                break;
            case MonsterSpwanPosition.Random:
                int random = Random.Range(0, 2);

                if (random == 1)
                {
                    MySpwanPoint = GetPoint(E_SpawnPoint.Low);
                }
                break;
            case MonsterSpwanPosition.Custom:
                MySpwanPoint = new Vector3(offsetx, offsety, 0);
                break;
        }
        return MySpwanPoint;
    }

    //위치 가져오기
    public Vector3 GetPoint(E_SpawnPoint spawnPoint)
    {
        return L_SpawnPoint[(int)spawnPoint];
    }

}

interface ISpawnPoint
{
    List<Vector3> L_SpawnPoint { get; set; }
    Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition, float offsetx, float offsety);
    Vector3 GetPoint(E_SpawnPoint spawnPoint);
}