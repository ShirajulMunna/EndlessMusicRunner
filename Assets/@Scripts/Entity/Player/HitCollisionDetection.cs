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
    const string PlayerUpEffectName = "Up_Effect";
    const string PlayerDownEffectName = "Down_Effect";
    public GameObject[] ScroeStateList;

    private List<string> ScoreStateListName = new List<string>()
    {"Perfect_Effect","Great_Effect","Great_Effect","Great_Effect","Opps_Effect"};

    [Range(0.2f, 10f)]
    public float fadeDuration = 2.0f;

    [SpineAnimation]
    public string HitAnimation;

    public Transform downHitPoint;
    public Transform upHitPoint;
    public float effectUpPositionY;

    //이펙트 위치 
    private enum EffectPosition
    {
        None,Up,Down,Middle
    }
    //판정 조건 오브젝트 생성
    private enum ConditionEffect
    {
        None = 0,Perfect,Great,Opps
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

    async void SetEffect(GameObject obj, ScoreManager.E_ScoreState perfect)
    {
        AudioManager.instance.PlaySound();
        //Perfect / Great�϶��� ���� �ְ� ���� 
        var score = 0;
        if (perfect == ScoreManager.E_ScoreState.Perfect)
        {
            score = 3;
        }
        else if (perfect == ScoreManager.E_ScoreState.Great)
        {
            score = 1;
        }
        ScoreManager.instance.SetCombo_Add();
        ScoreManager.instance.SetCurrentScore(score);

        if (perfect == ScoreManager.E_ScoreState.Late || perfect == ScoreManager.E_ScoreState.Early)
        {
            return;
        }

        var hitPoint = obj.transform.position;
        // ��� ����Ʈ �ϴ�����Ʈ ����

        if (hitPoint.y > 0)
        {
            var name = string.Format(AddresEffectName, (int)EffectPosition.Up);
            var result = await name.CreateOBJ<GameObject>(default, hitPoint, Quaternion.identity);
        }
        else if(obj.GetComponent<Monster>().uniqMonster == UniqMonster.SendBack)
        {
            var name = string.Format(AddresEffectName, (int)EffectPosition.Middle);
            var result = await name.CreateOBJ<GameObject>(default, hitPoint, Quaternion.identity);
        }
        else
        {
            var name = string.Format(AddresEffectName, (int)EffectPosition.Down);
            var result = await name.CreateOBJ<GameObject>(default,hitPoint,Quaternion.identity);
        }

        //switch(perfect)
        //{
        //    case ScoreManager.E_ScoreState.Perfect:
        //        var result1 = await Effect.Create(downHitPoint.position, (int)ConditionEffect.Perfect);
        //        result1.fadeDuration = fadeDuration;
        //        break;
        //    case ScoreManager.E_ScoreState.Great:
        //        var result2 = await Effect.Create(downHitPoint.position, (int)ConditionEffect.Great);
        //        result2.fadeDuration = fadeDuration;
        //        break;
        //    case ScoreManager.E_ScoreState.Miss:
        //        var result3 = await Effect.Create(downHitPoint.position, (int)ConditionEffect.Opps);
        //        result3.fadeDuration = fadeDuration;
        //        break;
        //}

        //var txteffects = ScroeStateList[(int)perfect];
        //CreatEffect(obj, hitPoint, (int)perfect, effectUpPositionY);

        var monsterType = obj.GetComponent<Monster>().uniqMonster;
        var effectPosition = Vector3.zero;
        if (hitPoint.y > 0 && monsterType == UniqMonster.Normal)
        {
            effectPosition = upHitPoint.position;
            effectPosition.y += effectUpPositionY;
        }
        else if (hitPoint.y > 0 && monsterType == UniqMonster.SendBack)
        {
            effectPosition = upHitPoint.position;
            effectPosition.y += effectUpPositionY;
        }
        else if (hitPoint.y < 0)
        {
            effectPosition = downHitPoint.position;
            effectPosition.y += effectUpPositionY;
        }
        var effect = ConditionEffect.None;
        switch (perfect)
        {
            case ScoreManager.E_ScoreState.Perfect:
                effect = ConditionEffect.Perfect;
                break;
            case ScoreManager.E_ScoreState.Great:
                effect = ConditionEffect.Great;
                break;
            case ScoreManager.E_ScoreState.Miss:
                effect = ConditionEffect.Opps;
                break;
        }
        var effects = await Effect.Create(effectPosition, (int)effect);
        effects.fadeDuration = fadeDuration;
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


    public void MoveUPword(GameObject perfectTxtEffect, Vector2 hitPoint)
    {
        perfectTxtEffect.transform.DOMoveY(hitPoint.y + 2, 0.1f);

    }
    public IEnumerator OpacityChange(GameObject obj)
    {
        var color = obj.GetComponent<SpriteRenderer>();
        Color currentColor = obj.GetComponent<SpriteRenderer>().color;


        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {

            if (obj == null || !obj.activeSelf)
                yield break;

            float normalizedTime = t / fadeDuration;


            currentColor.a = Mathf.Lerp(1, 0, normalizedTime);
            if (currentColor.a <= 0.1f)
            {
                currentColor.a = 0;
                color.color = currentColor;
                Destroy(obj);

                yield break;
            }

            if (obj != null)
                obj.GetComponent<SpriteRenderer>().color = currentColor;


            yield return null;
        }
    }

    private void CreatEffect(GameObject monster, Vector3 hitPoint, int txteffects, float effectUpPositionY)
    {
        var monsterType = monster.GetComponent<Monster>().uniqMonster;
        var effectPosition = Vector3.zero;
        if (hitPoint.y > 0 && monsterType == UniqMonster.Normal)
        {
            effectPosition = upHitPoint.position;
            effectPosition.y += effectUpPositionY;
        }
        else if (hitPoint.y > 0 && monsterType == UniqMonster.SendBack)
        {
            effectPosition = upHitPoint.position;
            effectPosition.y += effectUpPositionY;
        }
        else if (hitPoint.y < 0)
        {
            effectPosition = downHitPoint.position;
            effectPosition.y += effectUpPositionY;
        }
        //GameObject txtobject = Instantiate(txteffects, effectPosition, Quaternion.identity);
        //StartCoroutine(OpacityChange(txtobject));

        //MoveUPword(txtobject, effectPosition);
    }

    private void MakeEffectParticle(string name, Vector3 pos, Quaternion quaternion)
    {
        Addressables.InstantiateAsync(name, pos, quaternion);
    }
}
