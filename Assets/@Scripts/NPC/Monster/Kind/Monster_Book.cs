using UnityEngine;

public class Monster_Book : Monsters, IMonsterState
{
    public E_MonstersState e_MonstersState { get; set; }


    private void Update()
    {

    }

    public void SetState(E_MonstersState state)
    {
        switch (state)
        {
            case E_MonstersState.idle:
                break;
            case E_MonstersState.Hit:
                nPC_Move.SetSpeed(0);
                break;
            case E_MonstersState.Die:
                break;
            case E_MonstersState.Attack:
                break;
        }

        e_MonstersState = state;
    }


    public override void SetHit(int hp)
    {
        base.SetHit(hp);

    }
}

interface IMonsterState
{
    E_MonstersState e_MonstersState { get; set; }
}