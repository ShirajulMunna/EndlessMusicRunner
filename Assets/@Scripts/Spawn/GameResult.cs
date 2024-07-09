public class GameResult
{
    public void SetGameResult()
    {
        var gameresult = "Gameover_Score";
        var types = GameResultType.Clear;

        if (PlayerManager.instance.GetPlayer(0).nPC_Status.CheckDie())
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

