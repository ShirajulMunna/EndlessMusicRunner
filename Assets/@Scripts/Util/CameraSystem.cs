using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem cameraSystem;

    //기존 위치
    Vector3 orginPos = new Vector3(0f, 0.3f, -10f);
    float orginSize = 9;

    //줌인 사이즈 및 위치
    Vector3 ZoomInPos = new Vector3(-5, 0.3f, -10f);
    float ZoomInSize = 7;
    float Up_Down_Szie = 0.5f;

    //타겟
    float targetSize;
    Vector3 target;

    //카메라
    [SerializeField] Camera camera_main;

    private void Awake()
    {
        cameraSystem = this;
        targetSize = orginSize;
        target = this.transform.position;
    }

    private void Update()
    {
        SetZoom();
        SetMove();
    }

    void SetZoom()
    {
        var cursize = camera_main.orthographicSize;

        if (cursize >= targetSize)
        {
            camera_main.orthographicSize = targetSize;
            return;
        }

        camera_main.orthographicSize += Up_Down_Szie;
    }

    void SetMove()
    {
        var pos = transform.position;
        var updownsize = target.x > pos.x ? Up_Down_Szie : -Up_Down_Szie;

        var check = updownsize > 0 ? pos.x >= target.x : pos.x <= target.x;

        if (check)
        {
            return;
        }

        pos.x += updownsize;
        transform.position = pos;
    }

    public void SetZoomIn()
    {
        targetSize = ZoomInSize;
        target = ZoomInPos;
    }

    public void ReSetZoom()
    {
        targetSize = orginSize;
        target = orginPos;
    }
}
