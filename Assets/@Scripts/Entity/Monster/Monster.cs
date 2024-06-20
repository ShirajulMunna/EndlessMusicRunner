using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class Monster : Entity, IMonsterMove
{
    const string Name = "Monster_{0}";

    public static async Task<GameObject> Create(C_MonsterTable data, Vector3 cratepos, Transform Tr_Parent)
    {
        var name = string.Format(Name, data.PrefabName);
        var result = await name.CreateOBJ<Monster>(Tr_Parent);
        result.SetUp(data, cratepos);
        return result.gameObject;
    }

    [Header("공격력")]
    [SerializeField] protected int damageAmount;
    [Header("데미지 파티클")]
    [SerializeField] protected GameObject damageFx; //접근제한자 추가

    [Header("스파인 변경용")]
    [SerializeField] protected bool Change;
    [SerializeField] protected SkeletonDataAsset[] Sk;

    //죽었을때 날아가기
    private Vector3 randomDirection;
    const float FlySpeed = 50f; // 속도 조정 가능

    //몬스터 상태
    protected E_MonsterState e_MonsterState = E_MonsterState.idle;

    public System.Action Ac_Hit;
    public System.Action Ac_Die;

    public float Speed { get; set; }
    public float DestoryX { get; set; }

    private string HitAnimationNames = "Hit_Fly_1";
    // 샌드백일때 애니메이션 여러개 설정
    private List<string> HitRandAnimation = new List<string>()
    {
        "Hit_0","Hit_1","Hit_2","Hit_3","Hit_4"
    };
    protected PlayerSystem player
    {
        get => GameManager.instance.player;
    }
    protected IPlayer_KeyPoint Player_State
    {
        get => GameManager.instance.player.M_State;
    }
    public UniqMonster uniqMonster;

    protected virtual void Start()
    {
        if (Change)
        {
            skeletonAnimation.skeletonDataAsset = Sk[UI_Lobby.Type == false ? 0 : 1];
            skeletonAnimation.Initialize(true);
        }
    }

    protected virtual void Update()
    {
        var checkattack = CheckAttack();
        SetAttack(checkattack);
        SetDieFly();
        SetMove();
    }

    //초기화
    public virtual void SetUp(C_MonsterTable data, Vector3 cratepos)
    {
        CurHp = data.MaxHp;
        uniqMonster = data.Uniq_MonsterType;
        Speed = data.Speed;
        transform.position = cratepos;
        DestoryX = -100;
        damageAmount = data.Damage; //데미지 세팅해주는게 없어서 추가 
    }


    //공격 확인 함수
    protected virtual bool CheckAttack()
    {
        if (e_MonsterState == E_MonsterState.NoneAttack || e_MonsterState == E_MonsterState.Die)
        {
            return false;
        }

        var targetpos = player.transform.position;
        var offsetx = targetpos.x;
        if (offsetx < transform.position.x)
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

        //콤보 리셋 및 MISS추가 -함수화 함
        SetComboReset();

        var checkhit = CheckHitPoint();

        if (!checkhit)
        {
            return;
        }
        //이펙트추가 함수화 함 
        CreatPlayerHitEffect();
    }

    protected virtual bool CheckHitPoint()
    {
        //위치 맞는지 체크 후 공격
        var point = transform.position.y == -3.5f ? E_MovePoint.Down : E_MovePoint.Up;
        var checkhit = player.M_Move.CheckHitActive(point);
        return checkhit;
    }

    public override void SetHp(int value)
    {
        base.SetHp(value);
    }

    public virtual void SetHit(ScoreManager.E_ScoreState perfect)
    {
        Ac_Hit?.Invoke();
        e_MonsterState = E_MonsterState.NoneAttack;
        SetHp(-1);
        HitCollisionDetection.Instance.SetHit(this.gameObject, perfect);
        AudioManager.instance.PlaySound();

        // 샌드백일때 애니메이션 여러개 나오게하기
        if (uniqMonster == UniqMonster.SendBack)
        {
            int random = Random.Range(0, HitRandAnimation.Count - 1);
            skeletonAnimation.SetAni_Monster(HitRandAnimation[random], true);
        }

    }

    public override void SetDie()
    {
        base.SetDie();
        e_MonsterState = E_MonsterState.Die;
        Ac_Die?.Invoke();
        Speed = 0;
        bool isTrue = IsAnimationExists(HitAnimationNames);
        if (isTrue)
        {
            skeletonAnimation.SetAni_Monster(HitAnimationNames, true);
        }
        SetDieFlyDir();
        Destroy(this.gameObject, 0.5f);
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

    public void SetComboReset()
    {
        ScoreManager.instance.SetScoreState(ScoreManager.E_ScoreState.Miss);
        ScoreManager.instance.SetBestCombo_Reset();
    }
    public void CreatPlayerHitEffect()
    {
        GameObject opsFx = Instantiate(damageFx, transform.position, Quaternion.identity);
        Destroy(opsFx, 0.2f);
        AudioManager.instance.PlayerHItSound();
        player.SetHp(-damageAmount);
    }

    //몬스터중에서 애니메이션 있는지 검사
    private bool IsAnimationExists(string names)
    {
        List<string> animationNames = new List<string>(); //몬스터가 가지고있는 애니메이션이름 가져올예정
        if (skeletonAnimation != null && skeletonAnimation.SkeletonDataAsset != null)
        {
            var animationStateData = skeletonAnimation.SkeletonDataAsset.GetAnimationStateData();
            foreach (var animation in animationStateData.SkeletonData.Animations)
            {
                animationNames.Add(animation.Name);
            }
        }
        return animationNames.Contains(names);
    }

    public List<string> GetSpineAnimationNames(string name)
    {
        List<string> animationNames = new List<string>();

        if (skeletonAnimation != null && skeletonAnimation.Skeleton != null)
        {
            var animationStateData = skeletonAnimation.SkeletonDataAsset.GetAnimationStateData();
            foreach (var animation in animationStateData.SkeletonData.Animations)
            {
                if (animation.Name.Contains(name))
                    animationNames.Add(animation.Name);
            }
        }

        return animationNames;
    }
}