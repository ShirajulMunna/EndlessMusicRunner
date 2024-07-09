using UnityEngine;

public class Player_Effect : MonoBehaviour, IEffect
{
    /// <summary>
    /// 이펙트 생성
    /// </summary>
    public  async void SetEffect(Vector3 pos, ScoreManager.E_ScoreState state)
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
        await Effect.Create(pos, idx);
    }
}

interface IEffect
{
    void SetEffect(Vector3 pos, ScoreManager.E_ScoreState state);
}