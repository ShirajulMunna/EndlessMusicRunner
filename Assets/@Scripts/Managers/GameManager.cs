using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static GameManager instance;

    static Player _Player;
    public static Player M_Player
    {
        get
        {
            if (_Player == null)
            {
                _Player = FindObjectOfType<Player>();
            }

            return _Player;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    //################################################################### 리펙토리중





    public SkeletonAnimation skeleton;

    public PlayerSystem player;
    public Transform bossWaitPosition;

    public Transform lowerAttackPoint;
    [HideInInspector] public Vector3 longNoteDestoryPosition;

    public Vector3 GameResultPosition = new Vector3(10, -2, 0);


    // After playing some times background will
    // will change autometically .
    // Implement it here

    private UI_Pause pasueObject;

    //게임 결과 가져오기
    public void SetGameResult(GameResultType type)
    {
        Spine_GameResult.Create(GameResultPosition, type);
    }



    public void Update()
    {
        //Esc로 정지 기능 추가
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pasueObject != null)
            {
                pasueObject.Btn_ReStart();
                pasueObject = null;

            }
            else
                Btn_Pause();
        }
    }
    public async void Btn_Pause()
    {
        var name = "UI_Pause";
        pasueObject = await name.CreateOBJ<UI_Pause>();
    }
}