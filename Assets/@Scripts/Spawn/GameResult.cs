public class GameResult
{
    public void SetGameResult()
    {
        var gameresult = "Gameover_Score";
        var types = GameResultType.Clear;

        if (GameManager.instance.player.CurHp <= 0)
        {
            gameresult = "GameFail";
            types = GameResultType.Failed;
        }
        else if (ScoreManager.instance.IsPerfectState())
        {
            types = GameResultType.Full_combo;
        }

        AudioManager.instance.PlayEffectSound(gameresult);
        GameManager.instance.SetGameResult(types);
    }
}

interface IGameResult
{
    void SetGameResult();
}