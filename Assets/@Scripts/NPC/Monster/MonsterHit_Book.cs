using UnityEngine;

public class MonsterHit_Book : MonoBehaviour, IMonsterHit
{
    Monsters _MonsTer;
    public Monsters MonsTer
    {
        get
        {
            if (_MonsTer == null)
            {
                _MonsTer = GetComponent<Monsters>();
            }
            return _MonsTer;
        }
        set => _MonsTer = value;
    }

    public MonsterState monsterState
    {
        get => MonsTer.monsterState;
        set => monsterState = value;
    }

    public IMove nPC_Move
    {
        get => MonsTer.npc.nPC_Move;
        set => nPC_Move = value;
    }

    public float DelayTime { get; set; } = 0.3f;


    private void Start()
    {
        monsterState.AddStateAction_Start(E_MonstersState.Hit, SetState_Hit);
        monsterState.AddStateAction_Update(E_MonstersState.Hit, UpdateState_Hit);
    }

    public void SetState_Hit()
    {
        nPC_Move.SetSpeed(0);
        DelayTime = 0.3f;
    }

    public void UpdateState_Hit()
    {
        DelayTime -= Time.deltaTime;

        if (DelayTime > 0)
        {
            return;
        }
        nPC_Move.SetSpeed(20);
        monsterState.SetState(E_MonstersState.idle);
    }
}