using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    //게임 상태
    E_GameState e_GameState_;
    float DirDelayTime;
    float OffSetTime;

    //게임 오버 후 딜레이 시간
    const float gameOverTime_Result = 2f;
    //게임 오버 후 딜레이 시간
    const float gameOverTime_Delay = 4.5f;

    [SerializeField] string StrMusicFileName;

    public System.Action Ac_EndGame;
    System.Action Ac_MusicPlay;

    private void FixedUpdate()
    {
        spawnDelay?.GetDelayAction()?.Invoke();
        switch (e_GameState_)
        {
            case E_GameState.Play:
                UpdatePlay();
                UpdateEndCheck();
                break;
        }
    }

    public void SetState(E_GameState State)
    {
        switch (State)
        {
            case E_GameState.Wait:
                OffSetTime = spawnDelay.GetStartDelayTime();
                OffSetTime += 0.1f;
                break;
            case E_GameState.Play:
                Ac_MusicPlay += () => AudioManager.instance.PlayMusic();
                Ac_MusicPlay += () => Ac_MusicPlay = null;
                break;
            case E_GameState.End:
                System.Action action = () =>
                {
                    spawnDelay.Reset();
                    SetState(E_GameState.Result);
                };
                spawnDelay.SetDelay(gameOverTime_Delay, action, true);
                break;
            case E_GameState.GameOver:
                GameEnd();
                break;
            case E_GameState.Result:
                spawnCreate.AllDestoryMonster();
                Ac_EndGame?.Invoke();
                gameResult?.SetGameResult();
                spawnDelay.SetDelay(gameOverTime_Result, () => SetState(E_GameState.GameOver));
                break;
        }
        e_GameState_ = State;
    }


    //게임 시작
    public void PlayGame()
    {
        SetState(E_GameState.Wait);

        var bitname = string.IsNullOrEmpty(StrMusicFileName) ? UI_Lobby.Str_BitName : StrMusicFileName;

        spawnTimePoint.SetUp(bitname);
        spawnCreate.SetStart();
        DelayStart();
    }

    //딜레이 후 시작
    void DelayStart()
    {
        System.Action action = () =>
        {
            spawnDelay.Reset();
            SetState(E_GameState.Play);
        };
        spawnDelay.SetDelay(2, action, true);
    }

    //플레이
    void UpdatePlay()
    {
        DirDelayTime += Time.fixedDeltaTime;

        if (DirDelayTime <= OffSetTime)
        {
            SetCreate(DirDelayTime);
            return;
        }
        Ac_MusicPlay?.Invoke();
        var totaltime = AudioManager.instance.GetAudioTime() + OffSetTime;
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

    void UpdateEndCheck()
    {
        var check = spawnTimePoint.CheckEndTiems();
        if (!check)
        {
            return;
        }

        SetState(E_GameState.End);
    }

    //게임 종료 함수
    void GameEnd()
    {
        AudioManager.instance.StopMusic();
        UI_GameOver.Create();
    }


    //현재 게임상태 가져오기
    public E_GameState GetGameState()
    {
        return e_GameState_;
    }
}