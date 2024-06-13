using UnityEngine;

public class NoteTool : MonoBehaviour
{
    //비트 펄 미닛트 -> 120bpm이면 1분에 비트가 120번
    [SerializeField] int Bpm = 0;
    [SerializeField] GameObject Note;
    [SerializeField] Transform Tr_Create;
    [SerializeField] SpawnManager spawnManager;
    double curTime = 0d;

    int count = 0;

    private void Update()
    {
        curTime += Time.deltaTime;

        //60 /120 = 1비트당 0.5초 : 60s / BPM = 1 Beat시간
        if (curTime >= 60d / Bpm)
        {
            SetCreate();
            //0.5가 안되고 오차범위가 있기 때문에 0으로 초기화 하지 않음
            curTime -= 60d / Bpm;
        }
    }

    bool isCompletedSpawn = false;

    void SetCreate()
    {
        var level = GameData.Data.LevelDesigin[0];
        int counting = 0;
        bool isNewCreatBoss = false; // 보스생성했는가에 대한 변수
        for (int i = 0; i < level.Count; i++)
        {
            var monsterInfo = GameData.Data.MonsterTable[level[i].MonsterInfo];
            for (int j = 0; j < level[i].MonsterSpwanCount; ++j)
            {
                //보스 한번 생성했는지검사
                bool isBoss = monsterInfo.monsterType == Monster_Type.Boss;
                //보스 한번생성한적이 있다면 보스생성하지않고 돌아오는형태
                if (isBoss && isNewCreatBoss)
                    continue;

                spawnManager.MonsterSpawn(monsterInfo, (MonsterSpwanPosition)level[i].Spwan_Position);

                if (isBoss)
                    isNewCreatBoss = true;
            }
            i++;
        }
        //보스 포지션은 테스트후 재설정 할예정
        var bossPosition = Vector3.zero;
        bossPosition.x = -40;

        counting++;
        if (counting == 2)
        {
            isCompletedSpawn = true;
        }
    }
}