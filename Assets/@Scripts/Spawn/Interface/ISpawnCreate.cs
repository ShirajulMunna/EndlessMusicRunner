
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

interface ISpawnCreate
{
    List<GameObject> L_CreateData { get; set; }
    int CreateIDX { get; set; }
    void SetStart();
    Task<GameObject> MonsterSpawn(C_MonsterTable data, Vector3 createpoint);
    void SetActiveMonster();
}