using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_SendBack : MonoBehaviour
{
    [SerializeField] Monster monster;

    bool EndTimes;
    float DelayTime = 3;

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
        SetZoomOut();
        monster.Speed = 20;
        return;
    }


    //줌인 단계
    void SetZoomIn()
    {
        EndTimes = true;
        monster.Speed = 0;
        GameManager.instance.player.M_Attack.SetDirectMoveIdx(E_AttackPoint.Middle);
        CameraSystem.cameraSystem.SetZoomIn();
    }

    //줌아웃 단계
    void SetZoomOut()
    {
        GameManager.instance.player.M_Attack.SetDirectMoveIdx(E_AttackPoint.Down);
        CameraSystem.cameraSystem.ReSetZoom();
    }
}