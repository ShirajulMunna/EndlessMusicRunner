using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum E_GameOverState
{
    Success,
    Faild,
    NewScore
}

[Serializable]
public struct St_GameClear
{
    public GameObject[] G_List;
    public E_GameOverState e_GameState;
}

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
    [SerializeField] List<St_GameClear> L_GameState = new List<St_GameClear>();

    [SerializeField] TextMeshProUGUI[] T_TextList;
    [SerializeField] GameObject G_BestText;
    [SerializeField] GameObject G_LP;
    [SerializeField] GameObject[] G_RankSprite;
    private void Start()
    {
        SetUI();
        SetClear();
        SetName();
        SetScore();
        SetAccuracy();
        SetBestCombo();
        SetScoreState();
        SetRating();
    }

    //성공 실패 확인
    E_GameOverState GetGameState()
    {
        if (GameManager.instance.player.CheckDie())
        {
            return E_GameOverState.Faild;
        }

        if (ScoreManager.instance.GetBestCombo() < ScoreManager.instance.GetCurrentScore())
        {
            return E_GameOverState.NewScore;
        }
        return E_GameOverState.Success;
    }

    void SetUI()
    {
        var getgamestate = GetGameState();
        var data = L_GameState.Find(x => x.e_GameState == getgamestate);

        foreach (var item in L_GameState)
        {
            if (item.e_GameState == getgamestate)
            {
                foreach (var ites in item.G_List)
                {
                    ites.SetActive(true);
                }
            }
            else
            {
                foreach (var ites in item.G_List)
                {
                    ites.SetActive(false);
                }
            }
        }
    }

    //게임 클리어
    void SetClear()
    {
        StartCoroutine(IE_SetSound());
    }

    IEnumerator IE_SetSound()
    {
        var gamestate = GetGameState();
        yield return new WaitForSeconds(1f);

        switch (gamestate)
        {
            case E_GameOverState.Success:
            case E_GameOverState.NewScore:
                AudioManager.instance.PlayEffectSound("Gameover_Clear");
                break;
            case E_GameOverState.Faild:
                AudioManager.instance.PlayEffectSound("Gameover_Over");
                break;
        }
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
        var maxcount = (float)ScoreManager.instance.GetMaxState();
        var count = (float)ScoreManager.instance.GetAccuracy();
        //정확도 나누는형태로 변경완료
        var accuracy = ((count) / maxcount) * 100;
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
        //스코어 매니저에서 등급 받아서 랭크 이미지 출력
        var rank = ScoreManager.instance.GetScoreRank();

        G_RankSprite[(int)rank].gameObject.SetActive(true);
    }

    //재시작할때 혹시나 랭크 이미지 초기화해주기
    private void OffRankGameobject()
    {
        for(int i=0;i< G_RankSprite.Length;++i)
        {
            G_RankSprite[i].gameObject.SetActive(false);
        }
    }
    public void Btn_Exit()
    {
        ScoreManager.instance.ResetCount();
        OffRankGameobject();
        SecenManager.LoadScene("Lobby");
    }

    public void Btn_RePlay()
    {
        ScoreManager.instance.ResetCount();
        OffRankGameobject();
        SecenManager.LoadScene("MainGameScene");
    }
}