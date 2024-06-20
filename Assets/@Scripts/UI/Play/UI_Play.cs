using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Play : MonoBehaviour
{
    public static UI_Play Instance;

    [Header("키 설명")]
    public GameObject Key_Explain;

    [Header("콤보 오브젝트")]
    [SerializeField] GameObject combo;

    [Header("콤보 및 스코어 텍스트")]
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI comboTxt;

    [Header("HP")]
    [SerializeField] Image Img_Hp;

    [Header("Fever")]
    public Image Img_Fever;
    float DelayTime = 3;
    public bool GameOver;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivatPanel(bool activate)
    {
        ScoreManager.instance.ScoreReset();
        Key_Explain.SetActive(activate);
        StartCoroutine(DeactivatePanel());
    }

    IEnumerator DeactivatePanel()
    {
        yield return new WaitForSeconds(DelayTime);
        Key_Explain.SetActive(false);
        SpawnManager.instance.StartSpawningObjects();
    }

    //스코어 셋팅    
    public void SetScore(int score)
    {
        scoreTxt.text = score.ToString();
    }

    //콤보 리셋
    public void Reset_Combo()
    {
        combo.SetActive(false);
    }

    //콤보 셋팅
    public void Set_Combo(int score)
    {
        comboTxt.text = score.ToString();
        combo.SetActive(true);
    }

    public void SetHp(float max, float cur)
    {
        Img_Hp.fillAmount = cur / max;
    }

    public async void Btn_Pause()
    {
        var name = "UI_Pause";
        await name.CreateOBJ<UI_Pause>();
    }
}