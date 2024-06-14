#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    [SerializeField] int Bpm = 0;
    [SerializeField] Transform Tr_parent;

    bool CheckStart;
    int idx = 0;
    int Crateidx = 0;
    int CoolTime = 0;

    public bool isCompltedGame = false;
    float gameOverTime = 0f;
    public int StageInfo = 0;
    List<Vector3> L_SpawnPoint = new List<Vector3>()
    {
        new Vector3(20, -3.5f, 0),
        new Vector3(20, 0, 0),
        new Vector3(20, 3.5f, 0),
    };

    private double nextSpawnTime = 0d;

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

    private void FixedUpdate()
    {
        SetCreate();
    }

    private void Update()
    {
        GameEnd();
    }

    public void StartSpawningObjects()
    {
        CheckStart = true;
        isCompltedGame = false;
        nextSpawnTime = AudioSettings.dspTime;
        AudioManager.instance.PlayMusic();
    }

    public void StartSpawningObjectsEditor()
    {
#if UNITY_EDITOR
        CheckStart = true;
        isCompltedGame = false;
        nextSpawnTime = EditorApplication.timeSinceStartup;
        SetCreateEditor();
#endif
    }

    public void ResetSpawning()
    {
#if UNITY_EDITOR
        idx = 0;
        Crateidx = 0;
        CoolTime = 0;
        CheckStart = false;
        isCompltedGame = false;
        nextSpawnTime = 0d;
        XValue = 0;
        var children = new List<GameObject>();
        foreach (Transform child in Tr_parent)
        {
            children.Add(child.gameObject);
        }

        foreach (var child in children)
        {
            DestroyImmediate(child); // 즉시 삭제
        }
        Debug.Log("Spawning reset.");
#endif
    }

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
            CheckStart = false;
            isCompltedGame = false;
        }
    }

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

        double beatInterval = 60.0 / Bpm;
        double currentDspTime = AudioSettings.dspTime;

        if (currentDspTime < nextSpawnTime)
        {
            return;
        }

        while (currentDspTime >= nextSpawnTime)
        {
            nextSpawnTime += beatInterval;

            var level = GameData.Data.LevelDesigin[StageInfo];
            var idxs = GetIDX();

            if (idxs == -1)
            {
                return;
            }

            var monsterInfo = GameData.Data.MonsterTable[level[idxs].MonsterInfo];
            int spawnCount = level[idxs].MonsterSpwanCount;

            for (int i = 0; i < spawnCount; i++)
            {
                var createpoint = GetSpawnPoint((MonsterSpwanPosition)level[idxs].Spwan_Position);
                createpoint.x += 2f * i;
                MonsterSpawn(monsterInfo, createpoint);
            }

            Crateidx += spawnCount;
        }
    }

    float XValue;

    void SetCreateEditor()
    {
#if UNITY_EDITOR


        var level = GameData.Data.LevelDesigin[StageInfo];

        while (true)
        {
            if (isCompltedGame)
            {
                return;
            }

            if (!CheckStart)
            {
                return;
            }

            var idxs = GetIDX();

            if (idxs == -1)
            {
                XValue += 10.7f;
                continue;
            }
            var monsterInfo = GameData.Data.MonsterTable[level[idxs].MonsterInfo];
            int spawnCount = level[idxs].MonsterSpwanCount;

            for (int i = 0; i < spawnCount; i++)
            {
                var createpoint = GetSpawnPoint((MonsterSpwanPosition)level[idxs].Spwan_Position);
                createpoint.x += 2f * i + XValue; // x 값 조정
                MonsterSpawn(monsterInfo, createpoint);
                Debug.Log($"Spawned monster at {createpoint}. Monster index: {Crateidx + 1} / Xvalue{XValue}");
            }
            XValue += 10.7f;
            Crateidx += spawnCount;
        }
#endif
    }

    int GetIDX()
    {
        var level = GameData.Data.LevelDesigin[StageInfo];

        if (idx >= level.Count)
        {
            isCompltedGame = true;
            return -1;
        }

        var cratecount = level[idx].MonsterSpwanCount;

        if (Crateidx >= cratecount)
        {
            var cooltime = level[idx].CoolTime;

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

    public void MonsterSpawn(C_MonsterTable data, Vector3 createpoint)
    {
        Monster.Create(data, createpoint, Tr_parent);
    }

    Vector3 GetSpawnPoint(MonsterSpwanPosition spwanPosition)
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
        }
        return MySpwanPoint;
    }

    public Vector3 GetPoint(E_SpawnPoint spawnPoint)
    {
        return L_SpawnPoint[(int)spawnPoint];
    }
}
