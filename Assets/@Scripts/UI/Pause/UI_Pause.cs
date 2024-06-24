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
        //���ӵ��� ������ �ٽ� �÷����Ҷ� �������������� �� ������ �ʱ�ȭ
        ScoreManager.instance.ResetCount();
        ScoreManager.instance.ScoreReset();
    }
}