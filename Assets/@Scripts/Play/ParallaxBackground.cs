using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    float speed = 0.0002f;
    void FixedUpdate()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
