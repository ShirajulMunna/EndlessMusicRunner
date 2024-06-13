using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

public class Effect : MonoBehaviour
{
    const string Name = "GameConditionEffect_{0}";
    public float fadeDuration = 1f;
    public static async Task<Effect> Create(Vector3 spawnPosition,int number)
    {
        var name = string.Format(Name, number);
        var result = await name.CreateOBJ<Effect>(default,spawnPosition,default);
        return result;
    }
    void Start()
    {
        StartCoroutine(OpacityChange());

        MoveUPword();
    }
    public IEnumerator OpacityChange()
    {
        var color = GetComponent<SpriteRenderer>();
        Color currentColor = GetComponent<SpriteRenderer>().color;


        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {

            if (!gameObject.activeSelf)
                yield break;

            float normalizedTime = t / fadeDuration;


            currentColor.a = Mathf.Lerp(1, 0, normalizedTime);
            if (currentColor.a <= 0.1f)
            {
                currentColor.a = 0;
                color.color = currentColor;
                Destroy(this);

                yield break;
            }
            GetComponent<SpriteRenderer>().color = currentColor;


            yield return null;
        }
    }
    public void MoveUPword()
    {
        var hitPoint = transform.position;
        gameObject.transform.DOMoveY(hitPoint.y + 2, 0.1f);
    }
}
