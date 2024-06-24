using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnCreate : MonoBehaviour, ISpawnCreate
{
    public static SpawnCreate instance;
    const float MonsterOffSetX = 3;
    [SerializeField] Transform Tr_parent;

    SpawnStage spawnStage;
    SpawnPoint spawnPoint;

    public List<GameObject> L_CreateData { get; set; } = new List<GameObject>();
    public int CreateIDX { get; set; }

    //몬스터 생성 함수
    public async Task<GameObject> MonsterSpawn(C_MonsterTable data, Vector3 createpoint)
    {
        var result = await Monster.Create(data, createpoint, Tr_parent);
        return result;
    }
    private void Awake()
    {
        instance = this;
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
                // 2단 몬스터일때 y포지션 0으로 고정
                if (monsterInfo.Uniq_MonsterType == UniqMonster.TwinMonster)
                    createpoint.y = 0;

                var result = await MonsterSpawn(monsterInfo, createpoint);
                L_CreateData.Add(result);
                result.SetActive(false);
            }
        }
    }

    //생성한 몬스터 활성화
    public void SetActiveMonster()
    {
        if (L_CreateData.Count <= CreateIDX)
        {
            return;
        }

        L_CreateData[CreateIDX].SetActive(true);
        CreateIDX++;
    }

    //죽었을대 모든몬스터 가져오기
    public void AllDestoryMonster()
    {
        foreach (var item in L_CreateData)
        {
            item.SetActive(false);
        }
    }

}


interface ISpawnCreate
{
    List<GameObject> L_CreateData { get; set; }
    int CreateIDX { get; set; }
    void SetStart();
    Task<GameObject> MonsterSpawn(C_MonsterTable data, Vector3 createpoint);
    void SetActiveMonster();
}