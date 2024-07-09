using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnCreate : Singleton<SpawnCreate>, ISpawnCreate
{
    const float MonsterOffSetX = 3;

    SpawnStage spawnStage;
    SpawnPoint spawnPoint;
    public bool isStop;

    public List<IMonster> L_CreateData { get; set; } = new List<IMonster>();
    public int CreateIDX { get; set; }
    int CreateCount;

    //몬스터 생성 함수
    public async Task<IMonster> MonsterSpawn(C_MonsterTable data, Vector3 createpoint)
    {
        var result = await MonsterCreate.Create(data, createpoint, this.transform);
        return result;
    }

    private void Start()
    {
        spawnStage = GetComponent<SpawnStage>();
        spawnPoint = GetComponent<SpawnPoint>();
    }

    //몬스터 미리 생성
    public async void SetStart()
    {
        CreateIDX = 0;
        var level = spawnStage.GetLevelDesigns();
        var maxcount = level.Count;
        L_CreateData.Clear();
        for (int i = 0; i < maxcount; i++)
        {
            var currentLevel = level[i];
            var monsterInfo = GameData.Data.MonsterTable[currentLevel.MonsterInfo];
            int spawnCount = currentLevel.MonsterSpwanCount;
            var posstate = (MonsterSpwanPosition)currentLevel.Spwan_Position;
            var offsetx = currentLevel.OffSetX;
            var offsety = currentLevel.OffSetY;

            for (int j = 0; j < spawnCount; j++)
            {
                var createpoint = spawnPoint.GetSpawnPoint(posstate, offsetx, offsety);

                // 연속 생성일 시 처리
                createpoint.x += MonsterOffSetX * j;
                var result = await MonsterSpawn(monsterInfo, createpoint);
                L_CreateData.Add(result);
                result.npc.gameObject.SetActive(false);
                CreateCount++;
            }
        }
    }

    //생성한 몬스터 활성화
    public void SetActiveMonster()
    {
        if (CreateCount <= CreateIDX)
        {
            return;
        }

        L_CreateData[CreateIDX].SetActive();
        CreateIDX++;
    }

    //죽었을대 모든몬스터 가져오기
    public void AllDestoryMonster()
    {
        foreach (var item in L_CreateData)
        {
            if (item == null || item.monsterType == null)
            {
                continue;
            }

            if (item.monsterType.e_MonsterType == E_MonsterType.Boss)
            {
                continue;
            }
            
            if (item.npc == null)
            {
                continue;
            }

            item.npc.gameObject.SetActive(false);
        }
    }

}