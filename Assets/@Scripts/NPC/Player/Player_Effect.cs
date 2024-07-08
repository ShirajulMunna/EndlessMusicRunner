using UnityEngine;

public class Player_Effect : MonoBehaviour
{
    /// <summary>
    /// 이펙트 생성
    /// </summary>
    public void SetEffect(ScoreManager.E_ScoreState state)
    {
        var idx = 0;
        switch (state)
        {
            case ScoreManager.E_ScoreState.Perfect:
            case ScoreManager.E_ScoreState.Late:
            case ScoreManager.E_ScoreState.Early:
                idx = 1;
                break;
            case ScoreManager.E_ScoreState.Great:
                idx = 2;
                break;
            case ScoreManager.E_ScoreState.Miss:
                idx = 3;
                break;
            case ScoreManager.E_ScoreState.Pass:
                idx = 4;
                break;
        }

        Effect.Create(transform.position, idx);
    }
}

interface IEffect
{
    void SetEffect(ScoreManager.E_ScoreState state);
}