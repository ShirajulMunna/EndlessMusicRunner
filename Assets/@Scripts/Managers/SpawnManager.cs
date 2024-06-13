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
    bool CheckStart;
    int idx = 0;
    int Crateidx = 0;
    int CoolTime = 0;

    //게임 종료 확인
    bool isCompltedGame = false;


    float gameOverTime = 0f;
    public int StageInfo = 0; //추후 데이터 어디에서 데이터 받아오는 형태로 만들예정
    //생성 위치
    List<Vector3> L_SpawnPoint = new List<Vector3>()
    {
        new Vector3(20, -3.5f, 0),
        new Vector3(20, 0, 0),
        new Vector3(20, 3.5f, 0),
    };
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

    //게임 실행
    public void StartSpawningObjects()
    {
        CheckStart = true;
        isCompltedGame = false;
    }

    //게임 종료
    void GameEnd()
    {
        if (!isCompltedGame)
        {
            return;
        }

        gameOverTime += Time.deltaTime;

        if (gameOverTime >= 2.5f)
        {
            AudioManager.instance.StopMusic();
            UI_GameOver.Create();
            CheckStart = true;
            isCompltedGame = false;
        }
    }

    //생성 함수
    void SetCreate()
    {
        if (isCompltedGame)
        {
            return;
        }

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

    //IDX 가져오기
    int GetIDX()
    {
        var level = GameData.Data.LevelDesigin[StageInfo];
        //갯수 넘어갔나 체크
        if (idx >= level.Count)
        {
            //게임 종료
            isCompltedGame = true;
            return -1;
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

        return idx;
    }

    //몬스터 스폰
    public void MonsterSpawn(C_MonsterTable data, MonsterSpwanPosition spwanPosition)
    {
        //위치 생성
        var createpoint = GetSpawnPoint(spwanPosition);
        Monster.Create(data, createpoint);
    }

    //생성위치 가져오기
    Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition)
    {
        //위치 지정
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
        }
        return MySpwanPoint;
    }

    //스폰 위치 IDX
    public Vector3 GetPoint(E_SpawnPoint spawnPoint)
    {
        return L_SpawnPoint[(int)spawnPoint];
    }
}

