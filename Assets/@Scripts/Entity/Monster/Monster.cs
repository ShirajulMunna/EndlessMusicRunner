using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;


public class Monster : Entity, IMonsterMove
{
    public static void Create(string folderName, string name, Vector3 CreatePos, int hp, int speed, UniqMonster Uniq_MonsterType)
    {
        string path = $"{folderName}/{name}";
        var load = Resources.Load<GameObject>(path);
        var monster = Instantiate<GameObject>(load);
        var monsterValue = monster.GetComponent<Monster>();
        monsterValue.CurHp = hp;
        monsterValue.uniqMonster = Uniq_MonsterType;
        monsterValue.Speed = speed;
        monsterValue.transform.position = CreatePos;
    }

    [Header("공격력")]
    [SerializeField] int damageAmount;
    [Header("데미지 파티클")]
    [SerializeField] GameObject damageFx;

    [Header("스파인 변경용")]
    [SerializeField] protected bool Change;
    [SerializeField] protected SkeletonDataAsset[] Sk;

    //죽었을때 날아가기
    private Vector3 randomDirection;
    private float FlySpeed = 50f; // 속도 조정 가능

    //몬스터 상태
    protected E_MonsterState e_MonsterState = E_MonsterState.idle;

    public System.Action Ac_Hit;
    public System.Action Ac_Die;

    public float Speed { get; set; }
    public float DestoryX { get; set; }

    PlayerSystem player
    {
        get => GameManager.instance.player;
    }
    public UniqMonster uniqMonster;

    protected virtual void Start()
    {
        if (Change)
        {
            skeletonAnimation.skeletonDataAsset = Sk[UI_Lobby.Type == false ? 0 : 1];
            skeletonAnimation.Initialize(true);
        }
        Speed = 20;
        DestoryX = -100;
    }

    protected virtual void Update()
    {
        var checkattack = CheckAttack();
        SetAttack(checkattack);
        SetDieFly();
        SetMove();
    }

    //공격 확인 함수
    protected virtual bool CheckAttack()
    {
        if (e_MonsterState == E_MonsterState.NoneAttack || e_MonsterState == E_MonsterState.Die)
        {
            return false;
        }

        var targetpos = player.transform.position;

        if (targetpos.x < transform.position.x)
        {
            return false;
        }

        e_MonsterState = E_MonsterState.Attack;
        return true;
    }

    //기본적인 공격
    protected virtual void SetAttack(bool check)
    {
        if (!check)
        {
            return;
        }
        //더이상 공격 못하게 변경
        e_MonsterState = E_MonsterState.NoneAttack;

        //콤보 리셋 및 MISS추가
        ScoreManager.instance.SetScoreState(ScoreManager.E_ScoreState.Miss);
        ScoreManager.instance.SetBestCombo_Reset();

        //파티클 생성
        GameObject opsFx = Instantiate(damageFx, transform.position, Quaternion.identity);
        Destroy(opsFx, 0.2f);


        //위치 맞는지 체크 후 공격
        var point = transform.position.y == -3.5f ? E_AttackPoint.Down : E_AttackPoint.Up;
        var checkhit = player.M_Move.CheckHitActive(point);
        if (!checkhit)
        {
            return;
        }

        player.SetHp(-damageAmount);
    }

    public void SetHit(ScoreManager.E_ScoreState perfect)
    {
        Ac_Hit?.Invoke();
        e_MonsterState = E_MonsterState.NoneAttack;
        SetHp(-1);
        HitCollisionDetection.Instance.SetHit(this.gameObject, perfect);
    }

    public override void SetDie()
    {
        base.SetDie();
        e_MonsterState = E_MonsterState.Die;
        Ac_Die?.Invoke();
        Speed = 0;
        SetDieFlyDir();
        Destroy(this.gameObject, 1f);
    }

    //사망 시 위치 셋팅
    void SetDieFlyDir()
    {
        // 오른쪽으로 랜덤 방향 설정 (X 축 양수 방향)
        float randomY = Random.Range(-1f, 1f);
        randomDirection = new Vector3(1f, randomY, 0f).normalized; // 정규화된 벡터
    }

    //죽어서 날아가기
    void SetDieFly()
    {
        if (e_MonsterState != E_MonsterState.Die)
        {
            return;
        }
        // 매 프레임마다 위치를 업데이트
        transform.position += randomDirection * FlySpeed * Time.deltaTime;
    }

    //이동 함수
    public virtual void SetMove()
    {
        // 오브젝트를 왼쪽으로 이동
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
        // DestoryX 값이 0이면 -60, 아니면 DestoryX 사용
        var values = DestoryX;

        // 오브젝트가 특정 위치를 벗어나면 파괴
        if (transform.position.x < values)
        {
            Destroy(this.gameObject);
        }
    }
}