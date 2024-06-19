using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public enum ScoreRank
    {
        S,A,B,C,F,
    }
    public static ScoreManager _instance;
    public static ScoreManager instance
    {
        get
        {
            if (_instance == null)
            {
                var add = GameObject.CreatePrimitive(default).AddComponent<ScoreManager>();
                _instance = add;
            }
            return _instance;
        }
    }
    public int CurrentScore;
    public int BestScore;
    public int CurrentCombo;
    public int BestCombo;
    public Dictionary<E_ScoreState, int> D_SocreState = new Dictionary<E_ScoreState, int>();

    public enum E_ScoreState
    {
        Perfect,
        Great,
        Early,
        Late,
        Pass,
        Miss
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);

        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ScoreReset()
    {
        CurrentScore = 0;
        CurrentCombo = 0;
        BestCombo = 0;
    }

    //최고 콤보 가져오기
    public int GetBestCombo()
    {
        return BestCombo;
    }

    //콤보 추가
    public void SetCombo_Add()
    {
        CurrentCombo++;
        //현재 콤보가 5이상일때만 오브젝트 활성화 시키기
        if (CurrentCombo >= 5)
            UI_Play.Instance.Set_Combo(CurrentCombo);

        //피버타임 체크
        SkillSystem.instance.ActivePassiveSkill(SkillSystem.E_Trigger.Combo);

        //현재 콤보가 베스트 보다 큰지 확인
        if (CurrentCombo < BestCombo)
        {
            return;
        }
        BestCombo++;
    }

    //콤보 리셋
    public void SetBestCombo_Reset()
    {
        CurrentCombo = 0;
        UI_Play.Instance.Reset_Combo();
    }

    //점수 높이기
    public void SetCurrentScore(int vlaue)
    {
        var totalvalue = FerverTimeSystem.SetFeverScore(vlaue);
        CurrentScore += totalvalue;
        UI_Play.Instance.SetScore(CurrentScore);
    }
    //스킬 영향 없이 획득
    public void SetCurrentScore(int vlaue, bool noneadd)
    {
        CurrentScore += vlaue;
        UI_Play.Instance.SetScore(CurrentScore);
    }

    //현재 점수 가져오기
    public int GetCurrentScore()
    {
        return CurrentScore;
    }

    //이전 최고 점수 가져오기
    public int GetBestScore()
    {
        return BestScore;
    }

    //최고 점수 바꾸기
    public bool SetBestScore()
    {
        if (CurrentScore < BestScore)
        {
            return false;
        }
        BestScore = CurrentScore;
        return true;
    }

    //각 상태별 횟수 가져오기
    public int GetScoreState_Count(E_ScoreState state)
    {
        var check = D_SocreState.TryGetValue(state, out var data);
        return data;
    }

    //각 상태 카운트 추가
    public void SetScoreState(E_ScoreState state)
    {
        var check = D_SocreState.TryGetValue(state, out var data);

        if (!check)
        {
            D_SocreState.Add(state, 0);
        }

        D_SocreState[state]++;
    }

    //여태 나왔던 것들 총합
    public int GetMaxState()
    {
        var max = 0;

        foreach (var item in D_SocreState)
        {
            max += item.Value;
        }

        return max;
    }

    //미스 제외하고 총합
    public int GetAccuracy()
    {
        var count = 0;

        foreach (var item in D_SocreState)
        {
            if (item.Key == E_ScoreState.Miss)
            {
                continue;
            }
            count += item.Value;
        }

        return count;
    }
    //판정 모두 검사하는 형태로 작업
    public bool IsPerfectState()
    {
        return (GetAccuracy()/ GetMaxState()) ==1 ;
    }

    // 판정들 초기화
    public void ResetCount()
    {
        D_SocreState.Clear();
    }
    //퍼센트 결과값 출력

    public float GetRestultPersent()
    {
       return ((float)GetAccuracy() / (float)GetMaxState()) * 100;
    }
    //랭크 출력
    public ScoreRank GetScoreRank()
    {
        ScoreRank rank = ScoreRank.F;
        var accuracy = GetRestultPersent();

        if (accuracy >= 90.0f)
            rank = ScoreRank.S;
        else if (accuracy < 90.0f && accuracy >= 80.0f)
            rank = ScoreRank.A;
        else if (accuracy < 80.0f && accuracy >= 70.0f)
            rank = ScoreRank.B;
        else if (accuracy < 70.0f && accuracy >= 60.0f)
            rank = ScoreRank.C;

        return rank;
    }

}