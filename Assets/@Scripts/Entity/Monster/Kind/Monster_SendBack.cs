using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_SendBack : MonoBehaviour
{
    [SerializeField] Monster monster;

    bool EndTimes;
    bool DownType;
    float DelayTime = 0.5f;

    IPlayer_Move PlayerMove
    {
        get => GameManager.instance.player.M_Move;
    }


    private void Start()
    {
        monster.Ac_Hit += SetZoomIn;
        monster.Ac_Die += SetZoomOut;
    }

    // Update is called once per frame
    void Update()
    {
        SetMove();
    }

    void SetMove()
    {
        // EndTimes가 true이면 속도를 20으로 설정하고 종료
        if (!EndTimes)
        {
            return;
        }

        DelayTime -= Time.deltaTime;

        if (DelayTime > 0)
        {
            return;
        }
        if (!DownType)
        {
            SetZoomOut();
            DownType = true;
        }
        monster.Speed = 20;
        return;
    }

    //줌인 단계
    void SetZoomIn()
    {
        DownType = false;
        EndTimes = true;
        DelayTime = 0.3f;
        monster.Speed = 0;
        PlayerMove.SetDirrectMove(E_MovePoint.Middle);
    }

    //줌아웃 단계
    void SetZoomOut()
    {
        PlayerMove.SetDirrectMove(E_MovePoint.Down);
    }
}