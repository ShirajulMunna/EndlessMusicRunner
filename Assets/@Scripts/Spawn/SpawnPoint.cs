using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnPoint
{
    //스폰 위치
    public List<Vector3> L_SpawnPoint { get; set; } = new List<Vector3>()
    {
        new Vector3(20, 5f, 0),
        new Vector3(20, 3.5f, 0),
        new Vector3(20, 1, 0),
        new Vector3(20, -2.5f, 0),
        new Vector3(20, -5f, 0),
        new Vector3(20, -6.5f, 0),
    };

    public Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition, float offsetx, float offsety)
    {
        var MySpwanPoint = GetPoint(spwanPosition);

        switch (spwanPosition)
        {
            case MonsterSpwanPosition.Random:
                break;
            case MonsterSpwanPosition.Player:
                MySpwanPoint = GameManager.instance.player.transform.position;
                break;
            case MonsterSpwanPosition.Custom:
                MySpwanPoint = new Vector3(offsetx, offsety, 0);
                break;
        }
        return MySpwanPoint;
    }

    //위치 가져오기
    public Vector3 GetPoint(MonsterSpwanPosition spawnPoint)
    {
        return L_SpawnPoint[(int)spawnPoint];
    }

}

interface ISpawnPoint
{
    List<Vector3> L_SpawnPoint { get; set; }
    Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition, float offsetx, float offsety);
    Vector3 GetPoint(MonsterSpwanPosition spawnPoint);
}