using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DiskRotator : MonoBehaviour
{
    public static int ChoseBG;
    public GameObject charecterDisk;
    public void RotateDisk(float Z)
    {
        ChoseBG = Z > 0 ? ChoseBG + 1 : ChoseBG - 1;
        charecterDisk.transform.Rotate(0, 0, Z);
    }
}
