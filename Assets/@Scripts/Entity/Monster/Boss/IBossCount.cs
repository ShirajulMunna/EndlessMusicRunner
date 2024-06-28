public class BossCount : IBossCount
{
    public int PlayCount { get; set; }
    public E_Boss_Play_State PlayState { get; set; }

    public BossCount()
    {
        SetUp();
    }

    public void SetUp()
    {
        PlayCount = 2;
        PlayState = E_Boss_Play_State.Wait;
    }

    public bool CheckPlay()
    {
        PlayCount--;

        if (PlayCount > 0)
        {
            return false;
        }

        if (PlayState == E_Boss_Play_State.Complted)
        {
            return false;
        }

        PlayState = E_Boss_Play_State.Complted;

        return true;
    }
}


interface IBossCount
{
    E_Boss_Play_State PlayState { get; set; }
    int PlayCount { get; set; }
    void SetUp();
    bool CheckPlay();
}


public enum E_Boss_Play_State
{
    Wait,
    Play,
    Complted
}