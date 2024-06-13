using Spine.Unity;
using System.Collections;
using UnityEngine;


public class Boss : Monster
{
    public static void Create(Vector3 pos)
    {
        var load = Resources.Load<GameObject>("Boss");
        var boss = Instantiate<GameObject>(load);
        boss.transform.position = pos;

    }
}