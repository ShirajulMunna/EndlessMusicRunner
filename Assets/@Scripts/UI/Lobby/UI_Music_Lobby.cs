using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Music_Lobby : MonoBehaviour
{
    [SerializeField] RectTransform G_LP;
    [SerializeField] TextMeshProUGUI T_MusicTittle;
    [SerializeField] TextMeshProUGUI T_BestScore;

    float RotZ = 36f;

    private void Start()
    {
        var rot = GetRot(UI_Lobby.BitIdx, false);
        SetLp(rot);

    }

    private void OnEnable()
    {
        SetMusicTittle();
        SetBestScore();
    }

    public void Btn_RotLP(bool check)
    {
        var idx = GetIDX(check);
        var rot = GetRot(idx, check);
        SetLp(rot);
        UI_Lobby.instance.SetBit(idx);
        SetMusicTittle();
        SetBestScore();
    }

    void SetLp(float rot)
    {
        // 현재 회전에 RotZ 값을 더해 새로운 회전 생성
        Vector3 currentRotation = G_LP.rotation.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y, rot);

        // 새로운 회전 적용
        G_LP.rotation = Quaternion.Euler(newRotation);
    }

    int GetIDX(bool left)
    {
        var idx = UI_Lobby.BitIdx;
        idx = left ? idx - 1 : idx + 1;

        if (idx > UI_Lobby.instance.MaxBit())
        {
            idx = UI_Lobby.instance.MaxBit();
        }
        else if (idx < 0)
        {
            idx = 0;
        }

        return idx;
    }

    float GetRot(int idx, bool check)
    {
        var addrot = check ? RotZ : -RotZ;
        var rot = idx * addrot;
        return rot;
    }

    void SetMusicTittle()
    {
        T_MusicTittle.text = UI_Lobby.Str_BitName;
    }

    void SetBestScore()
    {
        T_BestScore.text = "BestScore: " + ScoreManager.instance.GetBestScore();
    }
}