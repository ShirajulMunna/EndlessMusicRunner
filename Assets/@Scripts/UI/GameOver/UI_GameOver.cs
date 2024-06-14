using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    const string Name = "UI_GameOver";
    public static async void Create()
    {
        if(UI_Play.Instance.GameOver)
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
    private void Start()
    {
        SetName();
        SetScore();
        SetAccuracy();
        SetBestCombo();
        SetScoreState();
        SetRating();
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
        for (int i = 0; i < curscore; i++)
        {
            dleay -= 0.03f;

            if(dleay <= 0)
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

        var accuracy = (float)count + (float)maxcount;

        T_TextList[3].text = "정확도 : " + accuracy + "%";
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
        T_TextList[11].text = "S";
    }

    public void Btn_Exit()
    {
        SecenManager.LoadScene("Lobby");
    }

    public void Btn_RePlay()
    {
        SecenManager.LoadScene("MainGameScene");
    }
}