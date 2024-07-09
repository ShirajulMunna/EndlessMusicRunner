using System;
using System.Collections;
using UnityEngine;

public class SpawnDelay : MonoBehaviour, ISpawnDelay
{
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
        set => _spawnPoint = value;
    }


    public float PlaterAttackZone { get; set; } = -11;
    public float MonsterSpeed { get; set; } = 20;

    public void SetDelay(float delay, System.Action action)
    {
        StartCoroutine(IE_Delay(delay, action));
    }

    public IEnumerator IE_Delay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    //생성 거리 딜레이 시간
    public float GetMonsterCreateDelay()
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

