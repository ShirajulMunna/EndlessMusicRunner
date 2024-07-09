using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Vector3 GameResultPosition = new Vector3(10, -2, 0);

    private UI_Pause pasueObject;

    private void Awake()
    {
        Application.targetFrameRate = 120;
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

    //게임 결과 가져오기
    public void SetGameResult(GameResultType type)
    {
        Spine_GameResult.Create(GameResultPosition, type);
    }

    public async void Btn_Pause()
    {
        var name = "UI_Pause";
        pasueObject = await name.CreateOBJ<UI_Pause>();
    }
}