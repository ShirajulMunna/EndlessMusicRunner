using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    //비트 펄 미닛트 -> 120bpm이면 1분에 비트가 120번
    [SerializeField] int Bpm = 0;
    double curTime = 0d;

    const double BpmDelay = 12d;

    bool isCompletedSpawn = false;
    float gameOverTime = 0f;
    public int StageInfo = 0; //추후 데이터 어디에서 데이터 받아오는 형태로 만들예정
    public enum E_SpawnPoint
    {
        Low,
        Middle,
        Hight,
        BossPoint,
    }

    List<Vector3> L_SpawnPoint = new List<Vector3>()
    {
        new Vector3(20, -3.5f, 0),
        new Vector3(20, 0, 0),
        new Vector3(20, 3.5f, 0),
       new Vector3(12, -2.5f, 0),
    };

    public List<GameObject> monsterOBjects = new List<GameObject>();
    public List<GameObject> bossObjects = new List<GameObject>();
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);

        }
        else
        {
            instance = this;
        }

    }

    private void Update()
    {
        GameEnd();
        SetCreate();
    }

    void GameEnd()
    {
        if (isCompletedSpawn)
        {
            gameOverTime += Time.deltaTime;

            if (gameOverTime >= 2.5f)
            {
                AudioManager.instance.StopMusic();
                UI_GameOver.Create();
                isCompletedSpawn = false;
            }
        }
    }

    bool CheckStart;
    int idx = 0;
    int Crateidx = 0;

    int CoolTime = 0;

    void SetCreate()
    {
        if (!CheckStart)
        {
            return;
        }

        curTime += Time.deltaTime;

        //24 /120 = 1비트당 0.2초 : 60s / BPM = 1 Beat시간
        if (curTime < BpmDelay / Bpm)
        {
            return;
        }
        //0.5가 안되고 오차범위가 있기 때문에 0으로 초기화 하지 않음
        curTime -= BpmDelay / Bpm;

        var level = GameData.Data.LevelDesigin[StageInfo];
        var idxs = GetIDX();

        if (idxs == -1)
        {
            return;
        }

        var monsterInfo = GameData.Data.MonsterTable[level[idxs].MonsterInfo];
        MonsterSpawn(monsterInfo, (MonsterSpwanPosition)level[idxs].Spwan_Position);
        Crateidx++;
    }

    int GetIDX()
    {
        var level = GameData.Data.LevelDesigin[StageInfo];
        //갯수 넘어갔나 체크
        if (idx >= level.Count)
        {
            //초기화
            idx = 0;
            Crateidx = 0;
            CoolTime = 0;
            return idx;
        }

        //생성 갯수 확인
        var cratecount = level[idx].MonsterSpwanCount;

        //생성 갯수가 넘었는지 체크
        if (Crateidx >= cratecount)
        {
            //쿨타임 체크
            var cooltime = level[idx].CoolTime;

            //쿨타임 있는지 체크
            if (cooltime > 0)
            {
                if (cooltime > CoolTime)
                {
                    CoolTime++;
                    return -1;
                }
            }
            CoolTime = 0;
            Crateidx = 0;
            idx++;
            return GetIDX();
        }

        //보스 한번 생성했는지검사
        var monsterInfo = GameData.Data.MonsterTable[level[idx].MonsterInfo];
        bool isBoss = monsterInfo.monsterType == Monster_Type.Boss;

        if (isBoss)
        {
            isNewCreatBoss = true;
        }

        //보스 한번생성한적이 있다면 보스생성하지않고 돌아오는형태
        if (isBoss && isNewCreatBoss)
        {
            CoolTime = 0;
            Crateidx = 0;
            idx++;
            return GetIDX();
        }

        return idx;
    }


    public void StartSpawningObjects(bool isSpawn)
    {
        CheckStart = true;
    }
    bool isNewCreatBoss = false; // 보스생성했는가에 대한 변수
    WaitForSeconds wait = new WaitForSeconds(0.1f);

    public void MonsterSpawn(C_MonsterTable data, MonsterSpwanPosition spwanPosition, bool CreatBoss = false)
    {
        if (data.Uniq_MonsterType == UniqMonster.SendBack)
        {
            Monster.Create(data, L_SpawnPoint[(int)E_SpawnPoint.Middle]);
            return;
        }

        //위치 지정
        var MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Hight];
        switch (spwanPosition)
        {
            case MonsterSpwanPosition.Down:
                MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                break;

            case MonsterSpwanPosition.Random:
                int random = Random.Range(0, 2);
                if (random == 1)
                {
                    MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                }
                break;
        }

        //보스인지 판단
        bool isMonster = data.monsterType == Monster_Type.Normal;

        if (isMonster)
        {
            Monster.Create(data, MySpwanPoint);
        }
        else if (!isMonster)
        {
            Boss.Create(L_SpawnPoint[(int)E_SpawnPoint.BossPoint]);
        }
    }

    public void LongNoteSpawn(C_MonsterTable t, MonsterSpwanPosition spwanPosition)
    {
        string prefab = string.Empty;
        prefab = monsterOBjects[t.PrefabName].name;

        var MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Hight];
        switch (spwanPosition)
        {
            case MonsterSpwanPosition.Down:
                MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                break;
            case MonsterSpwanPosition.Random:
                int random = Random.Range(0, 2);
                if (random == 1)
                    MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                break;
        }
        LongNote.Create("Monster", prefab, MySpwanPoint, t.Speed);
    }
}

