using Spine.Unity;
using System.Collections;
using UnityEngine;


public class Boss : MonoBehaviour 
{
    int AttackIdx = 0;
    public int speed;
    public int Damage;
    public int monsterMaxHp;
    public GameObject[] G_Pattern;

    [SerializeField] bool Change;
    [SerializeField] SkeletonDataAsset[] Sk;
    [SerializeField] SkeletonAnimation My;
    public enum E_State
    {
        Hit,
        Attack,
        Wait,
        PlayerMove,
    }

    E_State e_State = E_State.Wait;
    public float Delay = 1;
    float Cur_Delay = 0;

    public static Vector3 CreatePos;


    public SkeletonAnimation bossAnimation;
    public static void Create(Vector3 pos)
    {
        var load = Resources.Load<GameObject>("Boss");
        var boss = Instantiate<GameObject>(load);
        boss.transform.position = pos;

    }
    private void Start()
    {
        if (bossAnimation == null)
            bossAnimation = GetComponent<SkeletonAnimation>();

        if (Change)
        {
            My.skeletonDataAsset = Sk[UI_Lobby.Type == false ? 0 : 1];
            My.Initialize(true);
        }
        // GameManager.instance.PlayMonsterAnimation(bossAnimation, "Hit");
    }
    private void Update()
    {
        switch (e_State)
        {
            case E_State.Hit:
                Hit();
                break;
            case E_State.Attack:
                Pattern();
                break;
            case E_State.Wait:
                Wait();
                break;
            case E_State.PlayerMove:
                CrushPlayer();
                break;
        }
    }

    //패턴 공격
    void Pattern()
    {
        //공격 패턴 실행
        /* for(int i =0;i<G_Pattern.Length;++i)
         {
             GameObject spawnPoint = GameObject.Find("SpawnPoint_3");
             var obj = Instantiate(G_Pattern[AttackIdx], spawnPoint.transform.position,Quaternion.identity);
             Destroy(obj, 10);
             AttackIdx++;
         }*/
        StartCoroutine(spawnObjects());
        e_State = E_State.Wait;
        AttackIdx = G_Pattern.Length;

        if (AttackIdx >= G_Pattern.Length)
        {
            //돌격 패턴
            AttackIdx = 0;
            e_State = E_State.PlayerMove;
            return;
        }
    }
    //대기 상태
    void Wait()
    {
        return;
        Cur_Delay += Time.deltaTime;

        //일정 시간 뒤 공격
        if (Cur_Delay >= Delay)
        {
            e_State = E_State.Attack;
            Cur_Delay = 0;
            return;
        }
    }

    //맞았을때
    public void SetHit(ScoreManager.E_ScoreState perfact)
    {
        BossDamaged(1);
        e_State = E_State.Hit;
        HitCollisionDetection.Instance.SetHit(this.gameObject, perfact);
    }


    //플레이어에게 돌격
    void CrushPlayer()
    {
        var direction = GameManager.instance.player.transform.position;
        var playerPosition = direction;
        direction.y = transform.position.y;
        playerPosition.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        var dis = Vector3.Distance(playerPosition, transform.position);

        if (e_State == E_State.Hit)
        {
            return;
        }

        if (dis <= 1f)
        {
            //유저 공격 받는 코드 추가
            GameManager.instance.player.SetHp(-Damage);
            e_State = E_State.Hit;
        }
    }

    void Hit()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.bossWaitPosition.position, speed * Time.deltaTime);
        var dis = Vector3.Distance(transform.position, GameManager.instance.bossWaitPosition.position);

        if (dis <= 0.1f)
        {
            e_State = E_State.Wait;
            return;
        }
    }


    IEnumerator spawnObjects()
    {

        for (int i = 0; i < G_Pattern.Length; ++i)
        {
            yield return new WaitForSeconds(0.5f);


            GameObject spawnPoint = GameObject.Find("SpawnPoint_2");
            // GameObject sPoint = GameObject.Find("SpawnPoint_3");
            //sPoint.SetActive(false);
            var obj = Instantiate(G_Pattern[AttackIdx], spawnPoint.transform.position, Quaternion.identity);
            Destroy(obj, 10);
            AttackIdx++;
        }


    }
    public void BossDamaged(int damage)
    {
        monsterMaxHp -= damage;
        if (monsterMaxHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}