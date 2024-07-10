using JetBrains.Annotations;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] Player[] players;
    public System.Action Ac_Hit;

    bool isDie;
    float ClearDleay = 1f;
    System.Action Ac_Clear;

    
    private void Start()
    {
        PlayManager.instance.AddAction(E_Play.End, () =>
        {
            players[0].KeyReset();
            players[1].KeyReset();

            if (PlayerManager.instance.GetPlayer(0).nPC_Status.CheckDie())
            {
                return;
            }
            var pos = IMovePoint.GetMovePoint(E_MoveData.Higt_Low);
            pos.x = -2;
            players[0].nPC_Move.SetSpeed(20);
            players[0].nPC_Move.SetTarget(pos);


            pos = IMovePoint.GetMovePoint(E_MoveData.Low_Low);
            pos.x = 0;
            players[1].nPC_Move.SetSpeed(20);
            players[1].nPC_Move.SetTarget(pos);
            Ac_Clear = UpdateClear;
        });
    }

    private void Update()
    {
        Ac_Clear?.Invoke();
    }

    public void SetUp()
    {
        isDie = false;
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

    public void SyncDie()
    {
        if (isDie)
        {
            return;
        }
        isDie = true;
        players[0].SetDie();
        players[1].SetDie();
        PlayManager.instance.SetAction(E_Play.End);
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

    void UpdateClear()
    {
        ClearDleay -= Time.deltaTime;

        if (ClearDleay > 0)
        {
            return;
        }
        var rank = ScoreManager.instance.GetScoreRank();
        var strrank = players[0].player_Ani.GetAniString(E_AniKind_Player.Clear_S);
        switch (rank)
        {
            case ScoreManager.ScoreRank.A:
            case ScoreManager.ScoreRank.B:
                strrank = players[0].player_Ani.GetAniString(E_AniKind_Player.Clear_A);
                break;
            case ScoreManager.ScoreRank.C:
            case ScoreManager.ScoreRank.F:
                strrank = players[0].player_Ani.GetAniString(E_AniKind_Player.Clear_F);
                break;
        }

        players[0].player_Ani.SetAni(strrank, true, null);
        players[1].player_Ani.SetAni(strrank, true, null);
        Ac_Clear = null;
    }

}