using UnityEngine;

public class NPC : MonoBehaviour
{
    IStatus _nPC_Status;
    public IStatus nPC_Status
    {
        get
        {
            if (_nPC_Status == null)
            {
                _nPC_Status = GetComponent<IStatus>();
            }

            return _nPC_Status;
        }
    }
    IMove _nPC_Move;
    public IMove nPC_Move
    {
        get
        {
            if (_nPC_Move == null)
            {
                _nPC_Move = GetComponent<IMove>();
            }

            return _nPC_Move;
        }
    }

    IParticleSystem _nPC_ParticleSystem;
    protected IParticleSystem nPC_ParticleSystem
    {
        get
        {
            if (_nPC_ParticleSystem == null)
            {
                _nPC_ParticleSystem = GetComponent<IParticleSystem>();
            }

            return _nPC_ParticleSystem;
        }
    }

    public virtual void SetUp(int hp, float speed, int damage, Vector3 target)
    {
        nPC_Status?.SetUp(hp, speed, damage);
        nPC_Move?.SetTarget(target);
        nPC_ParticleSystem?.ActiveParticle(E_ParticleKind.Idle);
    }

    // 데미지 받음
    public virtual void SetHit(int hp)
    {
        nPC_Status?.SetHp(hp);

        if (!nPC_Status.CheckDie())
        {
            return;
        }
        SetDie();
    }

    // 공격
    public virtual void SetAttack(NPC target)
    {
        var damage = nPC_Status?.GetDamage();
        target.SetHit(-(int)damage);
    }

    // 사망시 처리
    public virtual void SetDie()
    {
        // 사망 처리 로직
    }

    // 위치 이동
    public virtual void SetMove(E_MoveData e_MoveData)
    {
        nPC_Move?.SetTarget(e_MoveData);
    }

    public virtual void SetMove(Vector3 pos)
    {
        nPC_Move?.SetTarget(pos);
    }
}
