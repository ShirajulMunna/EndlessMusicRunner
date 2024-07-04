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
            spawnDelay.SetDelay(4, UI_GameOver.Create);
        };
        PlayManager.instance.AddAction(E_Play.End, actions);
    }

    private void FixedUpdate()
    {
        UpdatePlay();
    }

    //게임 시작
    public void PlayGame()
    {
        var bitname = string.IsNullOrEmpty(StrMusicFileName) ? UI_Lobby.Str_BitName : StrMusicFileName;
        spawnTimePoint.SetUp(bitname);
        spawnCreate.SetStart();
        DelayStart();
    }


    //딜레이 후 시작
    void DelayStart()
    {
        spawnDelay.SetDelay(2, () => SetisStart(true));

        var delaytimes = spawnDelay.GetMonsterCreateDelay();
        spawnDelay.SetDelay(2 + delaytimes, AudioManager.instance.PlayMusic);
    }

    //플레이
    void UpdatePlay()
    {
        if (!isStart)
        {
            return;
        }
        var totaltime = AudioManager.instance.GetAudioTime();
        SetCreate(totaltime);
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
}