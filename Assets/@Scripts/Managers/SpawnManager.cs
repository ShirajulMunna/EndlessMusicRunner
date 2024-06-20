using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    //현재 스테이지 정보
    [SerializeField] int StageInfo = 0;
    //설정 Bpm
    [SerializeField] int Bpm = 0;
    //몬스터 소환 장소
    [SerializeField] Transform Tr_parent;

    //스폰 위치
    List<Vector3> L_SpawnPoint = new List<Vector3>()
    {
        new Vector3(20, -3.5f, 0),
        new Vector3(20, 0, 0),
        new Vector3(20, 3.5f, 0),
    };

    //생성 갯수 체크
    int CreatIDX;

    //몬스터 모음
    List<GameObject> L_CreateData = new List<GameObject>();

    //게임 오버 후 딜레이 시간
    float gameOverTime_Delay = 4.5f;
    //몬스터 생성 오프셋 위치값
    const float MonsterOffSetX = 2.5f;

    //다음 스폰 타임 계산
    float nextSpawnTime = 0f;

    [SerializeField] float StartMusic_Delay;
    bool CheckStartMusci_Delay;

    //게임 상태
    [SerializeField] E_GameState e_GameState = E_GameState.Wait;

    public System.Action Ac_EndGame;

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
        SetCreate();
    }

    private void FixedUpdate()
    {
        switch (e_GameState)
        {
            case E_GameState.Wait:
                break;
            case E_GameState.Start:
                SetActive();
                break;
            case E_GameState.End:
                break;
            case E_GameState.GameOver:
                break;
        }
    }

    private void Update()
    {
        switch (e_GameState)
        {
            case E_GameState.Wait:
                break;
            case E_GameState.Start:
                if (CheckStartMusci_Delay)
                {
                    return;
                }

                StartMusic_Delay -= Time.deltaTime;
                if (StartMusic_Delay > 0)
                {
                    return;
                }
                CheckStartMusci_Delay = true;
                AudioManager.instance.PlayMusic();
                break;
            case E_GameState.Result:
                Ac_EndGame?.Invoke();
                e_GameState = E_GameState.End;

                if (GameManager.instance.player.CurHp <= 0)
                {
                    CheckAllMonster();
                    GameManager.instance.SetGameResult(GameResultType.Failed);
                    return;
                }

                if (ScoreManager.instance.IsPerfectState())
                {
                    GameManager.instance.SetGameResult(GameResultType.Full_combo);
                    return;
                }

                GameManager.instance.SetGameResult(GameResultType.Clear);
                break;
            case E_GameState.End:

                //플레이어 체력 다 잃었을 경우
                gameOverTime_Delay -= Time.deltaTime;
                if (gameOverTime_Delay > 0)
                {
                    return;
                }
                GameEnd();
                break;
            case E_GameState.GameOver:
                break;
        }
    }


    //게임 시작시 몬스터 생성 함수 
    async void SetCreate()
    {
        var level = GameData.Data.LevelDesigin[StageInfo];

        var maxcount = level.Count;

        for (int i = 0; i < maxcount; i++)
        {
            var monsterInfo = GameData.Data.MonsterTable[level[i].MonsterInfo];
            int spawnCount = level[i].MonsterSpwanCount;

            for (int j = 0; j < spawnCount; j++)
            {
                var posstate = (MonsterSpwanPosition)level[i].Spwan_Position;
                var offsetx = level[i].OffSetX;
                var offsety = level[i].OffSetY;

                var createpoint = GetSpawnPoint(posstate, offsetx, offsety);

                //연속 생성일 시 처리
                createpoint.x += MonsterOffSetX * j;
                // 2단 몬스터일때 y포지션 0으로 고정
                if (monsterInfo.Uniq_MonsterType == UniqMonster.TwinMonster)
                    createpoint.y = 0;

                var result = await MonsterSpawn(monsterInfo, createpoint);
                L_CreateData.Add(result);
                result.SetActive(false);
            }

            //쿨타임 체크
            var cooltime = level[i].CoolTime;
            for (int k = 0; k < cooltime; k++)
            {
                L_CreateData.Add(null);
            }
        }
    }

    //게임 시작 초기화 함수
    public void StartSpawningObjects()
    {
        SetGameState(E_GameState.Start);
        nextSpawnTime = Time.time;
    }

    int LevelIDX;

    //몬스터 활성화
    void SetActive()
    {
        var level = GameData.Data.LevelDesigin[StageInfo];
        float beatInterval = 60.0f / Bpm;
        float currentTime = Time.time;

        if (currentTime < nextSpawnTime)
        {
            return;
        }

        //모두 생성 완료 됐다면 게임 종료
        if (CreatIDX >= L_CreateData.Count)
        {
            SetGameState(E_GameState.Result);
            return;
        }

        while (currentTime >= nextSpawnTime)
        {
            nextSpawnTime += beatInterval;

            //생성 갯수확인
            var cratecount = level[LevelIDX].MonsterSpwanCount;
            for (int i = 0; i < cratecount; i++)
            {
                if (L_CreateData[CreatIDX] == null)
                {
                    CreatIDX++;
                    return;
                }
                L_CreateData[CreatIDX].SetActive(true);
                CreatIDX++;
            }
            LevelIDX++;
        }
    }

    //게임 종료 함수
    void GameEnd()
    {
        AudioManager.instance.StopMusic();
        UI_GameOver.Create();
        SetGameState(E_GameState.GameOver);
    }

    //몬스터 생성 함수
    public async Task<GameObject> MonsterSpawn(C_MonsterTable data, Vector3 createpoint)
    {
        var result = await Monster.Create(data, createpoint, Tr_parent);
        return result;
    }

    //설정
    Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition, float offsetx, float offsety)
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


    //게임 상태 수정
    public void SetGameState(E_GameState state)
    {
        e_GameState = state;
    }

    //현재 게임상태 가져오기
    public E_GameState GetGameState()
    {
        return e_GameState;
    }

    //죽었을대 모든몬스터 가져오기
    public void CheckAllMonster()
    {
        var mon = FindObjectsOfType<Monster>();
        for (int i = 0; i < mon.Length; ++i)
        {
            mon[i].gameObject.SetActive(false);
        }
    }

    public int GetStageInfo()
    {
        return StageInfo;
    }
}
