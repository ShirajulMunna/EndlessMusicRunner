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

    public Transform downHitPoint;
    public Transform upHitPoint;
    public float effectUpPositionY = 1f;

    //이펙트 위치 
    private enum EffectPosition
    {
        None, Up, Down, Middle
    }
    //판정 조건 오브젝트 생성
    public enum ConditionEffect
    {
        None = 0, Perfect, Great, Opps
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


    void SetBoss(GameObject obj, ScoreManager.E_ScoreState perfect)
    {
        SetEffect(obj, perfect);
    }

    void SetEffect(GameObject obj, ScoreManager.E_ScoreState perfect)
    {
        var score = 0;
        if (perfect == ScoreManager.E_ScoreState.Perfect)
        {
            score = 3;
        }
        else if (perfect == ScoreManager.E_ScoreState.Great || perfect == ScoreManager.E_ScoreState.Pass)
        {
            score = 1;
        }
        ScoreManager.instance.SetCombo_Add();
        ScoreManager.instance.SetCurrentScore(score);

        CreatHitpartice(obj);
        CreateStateEffect(obj, perfect);
    }

    async void CreateStateEffect(GameObject obj, ScoreManager.E_ScoreState perfect)
    {
        var hitPoint = obj.transform.position;

        var effectPosition = upHitPoint.position;
        effectPosition.y += effectUpPositionY;

        if (hitPoint.y >= 0 && hitPoint.y <= 1)
        {
            effectPosition = downHitPoint.position;
            effectPosition.y -= effectUpPositionY * 3;
        }

        var effect = ConditionEffect.Great;
        switch (perfect)
        {
            case ScoreManager.E_ScoreState.Perfect:
                effect = ConditionEffect.Perfect;
                break;
            case ScoreManager.E_ScoreState.Miss:
                effect = ConditionEffect.Opps;
                break;
        }

        var effects = await Effect.Create(effectPosition, (int)effect);
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
        var tag = obj.tag;
        ScoreManager.instance.SetScoreState(state);
        if (obj.tag == "Monster")
        {
            SetEffect(obj, state);
        }
        else if (obj.tag == "Boss")
        {
            SetBoss(obj, state);
        }
    }
}
