using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossPattern_MoveAttack : MonoBehaviour, IPatern
{
    public Bosst bosst
    {
        get => Bosst.instance;
        set => Bosst.instance = value;
    }
    public bool isPlay { get; set; }

    private void Update()
    {
        CheckEnd();
    }

    public void PlayPattern()
    {
        SetTarget();
        bosst.iAni.SetAni(E_AniKind_Monster.Move, true);
        isPlay = true;
    }

    public void EndPattern()
    {
        bosst.nPC_Move.SetTarget(bosst.FirstPos());
        bosst.iAni.SetAni(E_AniKind_Monster.Move, true);
    }

    void CheckEnd()
    {
        if (!isPlay)
        {
            return;
        }

        //도착했는지 체크
        var check = bosst.nPC_Move.CheckIn();
        if (check)
        {
            return;
        }

        //종료 실행
        StartCoroutine(IE_End());
    }

    IEnumerator IE_End()
    {
        isPlay = false;
        bosst.iAni.SetAni(E_AniKind_Monster.Attack_0, false);

        yield return new WaitForSeconds(2f);
        //2초뒤 이동
        EndPattern();
    }

    void SetTarget()
    {
        var target = transform.position;
        target.x = IMovePoint.GetMonster_Attack_Point_X();
        bosst.nPC_Move.SetTarget(target);
    }
}

