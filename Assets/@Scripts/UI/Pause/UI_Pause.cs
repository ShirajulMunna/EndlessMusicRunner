using UnityEngine;

public class UI_Pause : MonoBehaviour
{
    const string Name = "UI_Pause";
    public static async void Create()
    {
        var result = await Name.CreateOBJ<UI_Pause>();
    }

    private void Start()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    public void Btn_ReStart()
    {
        Destroy(this.gameObject);
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void Btn_Exit()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SecenManager.LoadScene("Lobby");
        //게임도중 나가고 다시 플레이할때 누적판정개수들 및 점수들 초기화
        ScoreManager.instance.ResetCount();
        ScoreManager.instance.ScoreReset();
    }
}