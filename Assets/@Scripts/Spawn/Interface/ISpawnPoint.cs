

using UnityEngine;

interface ISpawnPoint
{
    System.Collections.Generic.List<Vector3> L_SpawnPoint { get; set; }
    Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition, float offsetx, float offsety);
    Vector3 GetPoint(E_SpawnPoint spawnPoint);
}