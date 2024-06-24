using System;
using UnityEngine;

public class SpawnDelay : MonoBehaviour, ISpawnDelay
{
    public float GameDelay { get; set; }
    public System.Action Ac_Delay { get; set; }

    SpawnPoint spawnPoint;

    const float PlaterAttackZone = -11;
    const float MonsterSpeed = 20;

    private void Start()
    {
        spawnPoint = GetComponent<SpawnPoint>();
    }

    public System.Action GetDelayAction()
    {
        return Ac_Delay;
    }

    public void SetDelay(float delay, System.Action action)
    {
        GameDelay = delay;
        Ac_Delay = action;
        Ac_Delay += Reset;
    }

    public void Reset()
    {
        Ac_Delay = null;
    }

    //딜레이 시간
    public float GetStartDelayTime()
    {
        var pointX = spawnPoint.GetPoint(0).x;
        var attackzone = PlaterAttackZone;
        var speed = MonsterSpeed;

        // 거리 계산
        var distance = Math.Abs(attackzone - pointX);

        // 시간 계산
        var time = distance / speed;

        return time;
    }

}

interface ISpawnDelay
{
    float GameDelay { get; set; }
    System.Action Ac_Delay { get; set; }
    System.Action GetDelayAction();
    void SetDelay(float delay, System.Action action);
    void Reset();
    float GetStartDelayTime();
}