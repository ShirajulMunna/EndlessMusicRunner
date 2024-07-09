using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ParticleSystem : MonoBehaviour, IParticleSystem
{
    [SerializeField] List<St_PlarticleKind> _ParticleKind;
    Dictionary<E_ParticleKind, List<St_PlarticleKind>> _d_ParticleKind;

    public Dictionary<E_ParticleKind, List<St_PlarticleKind>> D_ParticleKind
    {
        get
        {
            if (_d_ParticleKind == null)
            {
                SetUp();
            }
            return _d_ParticleKind;
        }
        set => _d_ParticleKind = value;
    }

    public Dictionary<E_ParticleKind, Coroutine> D_Active { get; set; } = new Dictionary<E_ParticleKind, Coroutine>();

    public Dictionary<float, WaitForSeconds> D_WaitTime { get; set; } = new Dictionary<float, WaitForSeconds>();

    public void SetUp()
    {
        _d_ParticleKind = new Dictionary<E_ParticleKind, List<St_PlarticleKind>>();

        foreach (var item in _ParticleKind)
        {
            if (!_d_ParticleKind.ContainsKey(item.ParticleType))
            {
                _d_ParticleKind[item.ParticleType] = new List<St_PlarticleKind>();
            }

            _d_ParticleKind[item.ParticleType].Add(item);
        }
    }

    //파티클 온
    public void ActiveParticle(E_ParticleKind kind, float times)
    {
        if (!D_ParticleKind.ContainsKey(kind) || D_ParticleKind[kind].Count == 0)
        {
            Debug.LogWarning($"Particle kind {kind} not found.");
            return;
        }

        if (D_Active.ContainsKey(kind))
        {
            // 이미 실행 중인 코루틴이 있다면 중지
            StopCoroutine(D_Active[kind]);
            D_Active.Remove(kind);
        }

        // 새 코루틴 시작
        Coroutine newCoroutine = StartCoroutine(IE_Active(kind, times));
        D_Active[kind] = newCoroutine;
    }

    //시간 맞춰 끄는 함수
    public IEnumerator IE_Active(E_ParticleKind kind, float times)
    {
        var data = D_ParticleKind[kind][0];
        var check = D_WaitTime.ContainsKey(times);
        if (!check)
        {
            D_WaitTime.Add(times, new WaitForSeconds(times));
        }

        var waitfor = D_WaitTime[times];
        foreach (var item in data.G_Particle)
        {
            item.SetActive(true);
        }

        //루프라면 끄는거 하지 않고 리턴
        var checkloop = data.Loop;
        if (checkloop)
        {
            yield break;
        }

        yield return waitfor;

        foreach (var item in data.G_Particle)
        {
            item.SetActive(false);
        }
    }
}