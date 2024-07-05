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
        //*!*! 게임 종료 시 멈추도록

        transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x <= lastPosition)
        {
            transform.position = pos;
        }
    }
}
