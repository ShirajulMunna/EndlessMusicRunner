using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeatingground : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool CheckisStop;
    float startPosition;
    float lastPosition = -250;
    Vector3 pos;

    private void Start()
    {
        pos = new Vector3(startPosition, transform.position.y);
    }

    void Update()
    {
        var conditioncheck_0 = SpawnManager.instance.GetGameState() == E_GameState.Result && speed >= 0.01f;
        var conditioncheck_1 = CheckisStop ? GameManager.instance.player.isStopPlayer : false;

        // ������ �����ٸ� ������ ���� 
        if (conditioncheck_0 || conditioncheck_1)
        {
            speed = 0.0f;
        }
        else if (GameManager.instance.player.CurHp <= 0)
            speed = 0.0f;
        transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x <= lastPosition)
        {
            transform.position = pos;
        }
    }
}
