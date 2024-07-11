using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    #region 클래스
    SpawnPoint _spawnPoint;
    SpawnPoint spawnPoint
    {
        get
        {
            if (_spawnPoint == null)
            {
                _spawnPoint = GetComponent<SpawnPoint>();
            }

            return _spawnPoint;
        }
    }

    SpawnCreate _spawnCreate;
    SpawnCreate spawnCreate
    {
        get
        {
            if (_spawnCreate == null)
            {
                _spawnCreate = GetComponent<SpawnCreate>();
            }

            return _spawnCreate;
        }
    }

    SpawnTimePoint _spawnTimePoint;
    SpawnTimePoint spawnTimePoint
    {
        get
        {
            if (_spawnTimePoint == null)
            {
                _spawnTimePoint = GetComponent<SpawnTimePoint>();
            }

            return _spawnTimePoint;
        }
    }

    SpawnDelay _spawnDelay;
    SpawnDelay spawnDelay
    {
        get
        {
            if (_spawnDelay == null)
            {
                _spawnDelay = GetComponent<SpawnDelay>();
            }

            return _spawnDelay;
        }
    }
    GameResult _gameResult;
    GameResult gameResult
    {
        get
        {
            if (_gameResult == null)
            {
                _gameResult = new GameResult();
            }
            return _gameResult;
        }
    }
    #endregion

    [SerializeField] string StrMusicFileName;
    bool isStart;
    float DelayStartTime;
    float UpdateDelayStartTime;

    private void Start()
    {
        PlayManager.instance.AddAction(E_Play.End, () => SetisStart(false));
        PlayManager.instance.AddAction(E_Play.End, AudioManager.instance.StopMusic);
        PlayManager.instance.AddAction(E_Play.End, spawnCreate.AllDestoryMonster);

        System.Action action = () =>
        {
            spawnDelay.SetDelay(2, gameResult.SetGameResult);
        };
        PlayManager.instance.AddAction(E_Play.End, action);

        System.Action actions = () =>
        {
            spawnDelay.SetDelay(3, UI_GameOver.Create);
        };
        PlayManager.instance.AddAction(E_Play.End, actions);
    }

    private void Update()
    {
        UpdateEndSpwan();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayManager.instance.SetAction(E_Play.End);
        }
    }

    private void FixedUpdate()
    {
        UpdatePlay();
    }

    //게임 시작
    public void PlayGame()
    {
        var bitname = string.IsNullOrEmpty(UI_Lobby.Str_BitName) ? StrMusicFileName : UI_Lobby.Str_BitName;
        spawnTimePoint.SetUp(bitname);
        spawnCreate.SetStart();
        DelayStart();
    }


    //딜레이 후 시작
    void DelayStart()
    {
        spawnDelay.SetDelay(2, () => SetisStart(true));

        DelayStartTime = spawnDelay.GetMonsterCreateDelay();
        DelayStartTime += 0.2f;//0.2초정도 
        spawnDelay.SetDelay(2 + DelayStartTime, AudioManager.instance.PlayMusic);
    }

    //플레이
    void UpdatePlay()
    {
        if (!isStart)
        {
            return;
        }

        UpdateDelayStartTime += Time.fixedDeltaTime;
        if (UpdateDelayStartTime >= DelayStartTime)
        {
            UpdateDelayStartTime = DelayStartTime;
        }

        var totaltime = AudioManager.instance.GetAudioTime();
        SetCreate(totaltime + UpdateDelayStartTime);
    }

    void SetCreate(double totaltime)
    {
        var check = spawnTimePoint.CheckTime(totaltime);
        if (!check)
        {
            return;
        }
        spawnTimePoint?.RemoveTimes();
        spawnCreate?.SetActiveMonster();
    }

    public void SetisStart(bool State)
    {
        isStart = State;
    }


    /// <summary>
    /// 게임 종료 확인
    /// </summary>
    void UpdateEndSpwan()
    {
        if (!isStart)
        {
            return;
        }

        var check = spawnTimePoint.CheckEndTiems();

        if (!check)
        {
            return;
        }
        PlayManager.instance.SetAction(E_Play.End);
        SetisStart(false);
    }
}