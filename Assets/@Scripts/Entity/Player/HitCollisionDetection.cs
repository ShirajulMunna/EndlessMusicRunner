using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

public class HitCollisionDetection : MonoBehaviour
{
    public static HitCollisionDetection Instance;

    [Range(0.2f, 10f)]
    public float fadeDuration = 2.0f;

    const float OffSetX_value = 2;

    //이펙트 위치 
    private enum EffectPosition
    {
        None, Up, Down, Middle
    }
    //판정 조건 오브젝트 생성
    public enum ConditionEffect
    {
        None = 0, Perfect, Great, Opps, PASS
    }
    const string AddresEffectName = "PlayerEffect_{0}";
    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);

        }
        else
        {
            Instance = this;
        }


    }

    //점수 처리 및 파티클 생성
    void SetEffect(GameObject obj, ScoreManager.E_ScoreState perfect)
    {
        var score = 0;

        switch (perfect)
        {
            case ScoreManager.E_ScoreState.Perfect:
            case ScoreManager.E_ScoreState.Early:
            case ScoreManager.E_ScoreState.Late:
                score = 3;
                break;
            case ScoreManager.E_ScoreState.Pass:
            case ScoreManager.E_ScoreState.Great:
                score = 1;
                break;
        }

        ScoreManager.instance.SetCombo_Add();
        ScoreManager.instance.SetCurrentScore(score);

        CreateStateEffect(perfect);

        if (perfect == ScoreManager.E_ScoreState.Pass)
        {
            return;
        }
        CreatHitpartice(obj);
    }

    //상태별 파티클 생성
    async void CreateStateEffect(ScoreManager.E_ScoreState perfect)
    {
        var hitPoint = GameManager.instance.player.transform.position;
        hitPoint.x += OffSetX_value;

        var effect = ConditionEffect.Opps;
        switch (perfect)
        {
            case ScoreManager.E_ScoreState.Perfect:
            case ScoreManager.E_ScoreState.Early:
            case ScoreManager.E_ScoreState.Late:
                effect = ConditionEffect.Perfect;
                break;

            case ScoreManager.E_ScoreState.Great:
                effect = ConditionEffect.Great;
                break;
            case ScoreManager.E_ScoreState.Pass:
                effect = ConditionEffect.PASS;
                break;
        }

        var effects = await Effect.Create(hitPoint, (int)effect);
        effects.fadeDuration = fadeDuration;
    }

    //히트 파티클 생성
    async void CreatHitpartice(GameObject obj)
    {
        string name = null;
        var hitPoint = obj.transform.position;

        //아래들 이펙트 생성및 파티클 생성
        if (hitPoint.y > 0)
        {
            name = string.Format(AddresEffectName, (int)EffectPosition.Up);
        }
        else if (obj.GetComponent<Monster>().uniqMonster == UniqMonster.SendBack)
        {
            name = string.Format(AddresEffectName, (int)EffectPosition.Middle);
        }
        else
        {
            name = string.Format(AddresEffectName, (int)EffectPosition.Down);
        }

        await name.CreateOBJ<GameObject>(default, hitPoint, Quaternion.identity);
    }

    public void SetHit(GameObject obj, ScoreManager.E_ScoreState state)
    {
        if (ScoreManager.E_ScoreState.Late == state)
        {
            Debug.Log("느림");
        }
        else if (ScoreManager.E_ScoreState.Early == state)
            Debug.Log("빠름");
        ScoreManager.instance.SetScoreState(state);
        SetEffect(obj, state);
    }
}
