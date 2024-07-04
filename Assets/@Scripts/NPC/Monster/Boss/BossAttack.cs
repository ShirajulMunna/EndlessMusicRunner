
using UnityEngine;

public class BossAttack : MonoBehaviour, IMonsterAttack
{
    public IMove move;
    public System.Action Ac_Attack { get; set; }
    public bool isCheckAttack { get; set; }

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

    private void Start()
    {
        move = GetComponent<IMove>();
    }

    private void Update()
    {
        UpdateAttack();
    }

    public void UpdateAttack()
    {
        if (isCheckAttack)
        {
            return;
        }

        var check = CheckAttack();

        if (!check)
        {
            return;
        }
        SetisAttack(true);
        Ac_Attack?.Invoke();
    }

    public bool CheckAttack()
    {
        return transform.position.x <= IMovePoint.GetMonster_Attack_Point_X();
    }

    /// <summary>
    /// 공격
    /// </summary>
    public void AddAttack(System.Action action)
    {
        Ac_Attack += action;
    }

    /// <summary>
    /// 공격 가능 여부
    /// </summary>
    public void SetisAttack(bool state)
    {
        isCheckAttack = state;
    }
}