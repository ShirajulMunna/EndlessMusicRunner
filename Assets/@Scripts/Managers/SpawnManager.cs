using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    SpawnPoint spawnPoint;
    SpawnCreate spawnCreate;
    SpawnTimePoint spawnTimePoint;
    SpawnDelay spawnDelay;
    GameResult gameResult;

    //게임 상태
    E_GameState e_GameState_;

    double PlayCurTime;
    float DirDelayTime;

    //게임 오버 후 딜레이 시간
    const float gameOverTime_Result = 2f;
    //게임 오버 후 딜레이 시간
    const float gameOverTime_Delay = 4.5f;

    public System.Action Ac_EndGame;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameResult = new GameResult();
        spawnCreate = GetComponent<SpawnCreate>();
        spawnPoint = GetComponent<SpawnPoint>();
        spawnTimePoint = GetComponent<SpawnTimePoint>();
        spawnDelay = GetComponent<SpawnDelay>();
    }


    private void FixedUpdate()
    {
        spawnDelay?.GetDelayAction()?.Invoke();
        switch (e_GameState_)
        {
            case E_GameState.Play:
                UpdatePlay();
                break;
        }
    }

    public void SetState(E_GameState State)
    {
        switch (State)
        {
            case E_GameState.Wait:
                DirDelayTime = spawnDelay.GetStartDelayTime();
                break;
            case E_GameState.Play:
                print(DirDelayTime);
                spawnDelay.SetDelay(DirDelayTime, () => AudioManager.instance.PlayMusic());
                break;
            case E_GameState.End:
                spawnDelay.SetDelay(gameOverTime_Delay, () => SetState(E_GameState.Result));
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
        spawnTimePoint.SetUp("엘라스타즈");
        spawnCreate.SetStart();
        DelayStart();
    }

    //딜레이 후 시작
    void DelayStart()
    {
        System.Action action = () =>
        {
            SetState(E_GameState.Play);

        };
        spawnDelay.SetDelay(2, action);
    }

    //플레이
    void UpdatePlay()
    {
        PlayCurTime += Time.fixedDeltaTime;
        var check = spawnTimePoint.CheckTime(PlayCurTime);
        if (!check)
        {
            return;
        }
        spawnTimePoint?.RemoveTimes();
        spawnCreate?.SetActiveMonster();
    }

    //게임 종료 함수
    void GameEnd()
    {
        AudioManager.instance.StopMusic();
        UI_GameOver.Create();
        SetState(E_GameState.GameOver);
    }


    //현재 게임상태 가져오기
    public E_GameState GetGameState()
    {
        return e_GameState_;
    }
}