using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IParticleSystem
{
    Dictionary<E_ParticleKind, Coroutine> D_Active { get; set; }
    Dictionary<float, WaitForSeconds> D_WaitTime { get; set; }
    Dictionary<E_ParticleKind, List<St_PlarticleKind>> D_ParticleKind { get; set; }
    void SetUp();
    IEnumerator IE_Active(E_ParticleKind kind, float times);
    void ActiveParticle(E_ParticleKind kind, float times);
}