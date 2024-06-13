using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeatingground : MonoBehaviour
{
    [SerializeField] float speed;
    float startPosition;
    float lastPosition = -250;
    Vector3 pos;

    private void Start()
    {
        pos = new Vector3(startPosition, transform.position.y);
    }

    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x <= lastPosition)
        {
            transform.position = pos;
        }
    }
}
