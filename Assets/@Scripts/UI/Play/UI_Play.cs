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
    [SerializeField] GameObject G_HP_BackGorund;
    [Header("Fever")]
    public Image Img_Fever;
    float DelayTime = 3;
    public bool GameOver;

    public System.Action Ac_Update;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Ac_Update?.Invoke();
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
        var per = cur / max;
        Img_Hp.fillAmount = per;
        SetHP_BackGround(per);
    }

    void SetHP_BackGround(float per)
    {
        G_HP_BackGorund.SetActive(per <= 0.25f);
    }


    public async void Btn_Pause()
    {
        var name = "UI_Pause";
        await name.CreateOBJ<UI_Pause>();
    }

    public void SetFever(int combo)
    {
        Img_Fever.fillAmount = (float)ScoreManager.instance.GetCurrentScore() / (float)combo;
    }

    public void SetMinusFever(float max, float cur)
    {
        cur -= Time.deltaTime;
        Img_Fever.fillAmount = (float)cur / (float)max;
    }
}