using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPointRotation : MonoBehaviour
{
    float speed = 100;
    Vector3 rot = Vector3.zero;


    void Update()
    {
        rot.z = speed * Time.deltaTime;
        transform.Rotate(rot);
    }
}
