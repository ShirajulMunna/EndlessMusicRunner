using System;
using UnityEngine;

[Serializable]
public struct St_PlarticleKind
{
    public E_ParticleKind ParticleType;
    public GameObject[] G_Particle;
    public bool Loop;
}