

using System.Collections.Generic;
using UnityEngine;

interface ISpawnPoint
{
    Dictionary<E_MoveData, Vector3> D_MovePoint { get; set; }
    Vector3 GetSpawnPoint(E_MoveData spwanPosition, float offsetx, float offsety);
    Vector3 GetPoint(E_MoveData spawnPoint);
}