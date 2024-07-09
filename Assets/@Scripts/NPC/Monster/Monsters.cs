using System.Threading.Tasks;
using UnityEngine;

public class Monsters : NPC, IMonster
{
    public System.Action Ac_Active { get; set; }
    IMonsterType _monster_Type_Data;
    public IMonsterType monsterType
    {
        get
        {
            if (_monster_Type_Data == null)
            {
                _monster_Type_Data = GetComponent<IMonsterType>();
            }
            return _monster_Type_Data;
        }
        set => _monster_Type_Data = value;
    }

    IMonsterAttack _monster_Attack;
    public IMonsterAttack monsterAttack
    {
        get
        {
            if (_monster_Attack == null)
            {
                _monster_Attack = GetComponent<MonsterAttack>();
            }
            return _monster_Attack;
        }
        set => _monster_Attack = value;
    }

    IMonsterAni _ani;
    public IMonsterAni iAni
    {
        get
        {
            if (_ani == null)
            {
                _ani = GetComponent<IMonsterAni>();
            }

            return _ani;
        }
        set => _ani = value;
    }

    NPC _nPC;
    public NPC npc
    {
        get => this;
        set => _nPC = value;
    }

    MonsterState _monsterState;
    public MonsterState monsterState
    {
        get
        {
            if (_monsterState == null)
            {
                _monsterState = GetComponent<MonsterState>();
            }
            return _monsterState;
        }
        set => _monsterState = value;
    }

    /// <summary>
    /// 몬스터 생성 처리
    /// </summary>
    public void CreateMonster(C_MonsterTable data, Vector3 cratepos)
    {
        GameLog.Log($"몬스터 HP{data.MaxHp}");
        //생성 위치 고정    
        transform.position = cratepos;

        //타입 지정
        monsterType?.SetMonsterType((E_MonsterType)data.Uniq_MonsterType);
        GameLog.Log($"몬스터 타입: {(E_MonsterType)data.Uniq_MonsterType}");

        //공격 셋팅
        System.Action action = () =>
        {
            var target_npc = GameManager.M_Player;
            SetAttack(target_npc);
        };
        monsterAttack?.AddAttack(action);

        Ac_Active = () =>
        {
            SetUp(data.MaxHp, data.Speed, data.Damage, SetMoveTarget());
        };
    }

    //이동 위치 타겟
    public virtual Vector3 SetMoveTarget()
    {
        //이동 위치 셋팅
        var target = transform.position;
        target.x = IMovePoint.GetMax_X_value();
        return target;
    }

    /// <summary>
    /// 사망 시 위치 셋팅
    /// </summary>
    public virtual void SetDieMonster()
    {
        // 오른쪽으로 랜덤 방향 설정 (X 축 양수 방향)
        float randomY = Random.Range(-1f, 1f);
        float randomX = Random.Range(-1f, 1f);
        var target = new Vector3(randomX, randomY, 0f).normalized; // 정규화된 벡터
        nPC_Move?.SetTarget(target);
    }

    /// <summary>
    /// 오브젝트 켰을때 처리
    /// </summary>
    public virtual void SetActive()
    {
        this.gameObject.SetActive(true);
        Ac_Active?.Invoke();
    }

    public override void SetUp(int hp, float speed, int damage, Vector3 target)
    {
        base.SetUp(hp, speed, damage, target);
    }

    public override void SetDie()
    {
        base.SetDie();
        SetDieMonster();
        //사망 애니메이션
        iAni?.SetAni(E_AniKind_Monster.Die, true);
        nPC_Move.SetSpeed(100);
        Destroy(this.gameObject, 0.1f);
    }

    public override void SetHit(int hp)
    {
        base.SetHit(hp);
        iAni?.SetAni(E_AniKind_Monster.Hit_0, true);
        monsterState?.SetState(E_MonstersState.Hit);
    }

    public override void SetAttack(NPC target)
    {
        base.SetAttack(target);
        iAni?.SetAni(E_AniKind_Monster.Attack_0, true);
        monsterState?.SetState(E_MonstersState.Attack);
    }
}