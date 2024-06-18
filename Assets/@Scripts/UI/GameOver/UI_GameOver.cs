using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    const string Name = "UI_GameOver";
    public static async void Create()
    {
        if (UI_Play.Instance.GameOver)
        {
            return;
        }

        UI_Play.Instance.GameOver = true;
        var obj = await Name.CreateOBJ<UI_GameOver>();
        var audio = AudioManager.instance;
        audio.Audio_BackGround.PlayOneShot(audio.failGame);
    }

    [SerializeField] TextMeshProUGUI[] T_TextList;
    [SerializeField] GameObject G_BestText;
    [SerializeField] GameObject G_LP;
    private float rate = 0f;
    private void Start()
    {
        SetClear();
        SetName();
        SetScore();
        SetAccuracy();
        SetBestCombo();
        SetScoreState();
        SetRating();
    }

    //성공 실패 확인
    bool CheckClear()
    {
        return GameManager.instance.player.CheckDie();
    }

    //게임 클리어
    void SetClear()
    {
        StartCoroutine(IE_SetSound());
    }

    IEnumerator IE_SetSound()
    {
        var checkclear = CheckClear();


        yield return new WaitForSeconds(1f);

        //사망 처리
        if (checkclear)
        {
            AudioManager.instance.PlayEffectSound("Gameover_Over");
            yield break;
        }

        //클리어처리
        AudioManager.instance.PlayEffectSound("Gameover_Clear");
    }

    void SetName()
    {
        T_TextList[0].text = "엘라스타즈";
    }

    //스코어 셋팅
    void SetScore()
    {
        StartCoroutine(IE_Score());
        T_TextList[2].text = "BEST : " + ScoreManager.instance.GetBestScore().ToString();
        var best = ScoreManager.instance.SetBestScore();
        G_BestText.SetActive(best);
    }

    IEnumerator IE_Score()
    {
        var wait = new WaitForSeconds(0.03f);
        var curscore = ScoreManager.instance.GetCurrentScore();
        var dleay = 1f;
        AudioManager.instance.PlayEffectSound("Gameover_Score");
        for (int i = 0; i < curscore; i++)
        {
            dleay -= 0.03f;

            if (dleay <= 0)
            {
                break;
            }

            yield return wait;
            T_TextList[1].text = i.ToString();
        }
        T_TextList[1].text = curscore.ToString();
        G_LP.SetActive(true);
    }

    void SetAccuracy()
    {
        var maxcount = ScoreManager.instance.GetMaxState();
        var count = ScoreManager.instance.GetAccuracy();
        //정확도 나누는형태로 변경완료
        var accuracy = ((float)(count) / (float)maxcount) * 100;
        rate = accuracy;
        T_TextList[3].text = "정확도 : " + accuracy.ToString("F2") + "%";
    }

    void SetBestCombo()
    {
        T_TextList[4].text = "최고 콤보수\n" + ScoreManager.instance.GetBestCombo().ToString();
    }

    void SetScoreState()
    {
        T_TextList[5].text = "PERFECT : " + ScoreManager.instance.GetScoreState_Count(ScoreManager.E_ScoreState.Perfect).ToString();
        T_TextList[6].text = "GREAT : " + ScoreManager.instance.GetScoreState_Count(ScoreManager.E_ScoreState.Great).ToString();
        T_TextList[7].text = "EARLY : " + ScoreManager.instance.GetScoreState_Count(ScoreManager.E_ScoreState.Early).ToString();
        T_TextList[8].text = "LATE : " + ScoreManager.instance.GetScoreState_Count(ScoreManager.E_ScoreState.Late).ToString();
        T_TextList[9].text = "PASS : " + ScoreManager.instance.GetScoreState_Count(ScoreManager.E_ScoreState.Pass).ToString();
        T_TextList[10].text = "MISS : " + ScoreManager.instance.GetScoreState_Count(ScoreManager.E_ScoreState.Miss).ToString();
    }

    void SetRating()
    {
        //판정 결과 S~C 이후  F까지
        string str = string.Empty;
        if (rate >= 90.0f)
            str = "S";
        else if (rate < 90.0f && rate >= 80.0f)
            str = "A";
        else if (rate < 80.0f && rate >= 70.0f)
            str = "B";
        else if (rate < 70.0f && rate >= 60.0f)
            str = "C";
        else
            str = "F";
        T_TextList[11].text = str;
    }

    public void Btn_Exit()
    {
        ScoreManager.instance.ResetCount();
        SecenManager.LoadScene("Lobby");
    }

    public void Btn_RePlay()
    {
        ScoreManager.instance.ResetCount();
        SecenManager.LoadScene("MainGameScene");
    }
}