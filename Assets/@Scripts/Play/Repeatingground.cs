using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeatingground : MonoBehaviour
{
    [SerializeField] float speed;
    float startPosition;
    float lastPosition = -250;
    Vector3 pos;
    bool isStop;

    private void Start()
    {
        pos = new Vector3(startPosition, transform.position.y);
        ActionManager.instance.AddAction((E_ActionScene.Play, E_ActionList.Boss), () => isStop = true);
    }

    void Update()
    {
        if (isStop)
        {
            return;
        }

        transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x <= lastPosition)
        {
            transform.position = pos;
        }
    }
}
