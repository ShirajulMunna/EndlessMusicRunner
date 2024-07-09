using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] Player[] players;
    public System.Action Ac_Hit;

    private void Start()
    {
        players[0].SetKeyInput(KeyCode.D, KeyCode.F, true);
        players[1].SetKeyInput(KeyCode.J, KeyCode.K, false);
    }

    public void SetHit()
    {
        Ac_Hit?.Invoke();
        Sethp();
    }

    /// <summary>
    /// HP동기화
    /// </summary>
    void Sethp()
    {
        var hp_1 = players[0].nPC_Status.GetHp();
        var hp_2 = players[1].nPC_Status.GetHp();
        var hp = 0;
        if (hp_1 > hp_2)
        {
            hp = hp_1 - hp_2;
            players[0].nPC_Status.SetHp(-hp);
            return;
        }
        hp = hp_2 - hp_1;
        players[1].nPC_Status.SetHp(-hp);
    }

    public Player GetPlayer(float yvalue)
    {
        var point = IMovePoint.GetPoint()[E_MoveData.Low_Higt];
        var check = yvalue > point.y;

        return check ? players[0] : players[1];
    }

    public Player GetPlayer(int idx)
    {
        return players[idx];
    }
}