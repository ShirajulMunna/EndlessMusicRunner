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


    public enum E_State
    {
        Hit,
        Attack,
        Wait,
        PlayerMove,
    }

    E_State e_State = E_State.Wait;
    float Delay = 3;
    float Cur_Delay = 0;

    public static Vector3 CreatePos;


    public SkeletonAnimation bossAnimation;
    public static void Create()
    {
        var load = Resources.Load<GameObject>("Boss");
        var boss = Instantiate<GameObject>(load);
        boss.transform.position = CreatePos;

    }
    private void Start()
    {
        if (bossAnimation == null)
            bossAnimation = GetComponent<SkeletonAnimation>();

        GameManager.instance.PlayMonsterAnimation(bossAnimation, "Hit");
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
        for(int i =0;i<G_Pattern.Length;++i)
        {
            var obj = Instantiate(G_Pattern[AttackIdx]);
            Destroy(obj, 10);
            AttackIdx++;
        }
        e_State = E_State.Wait;
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
        Cur_Delay += Time.deltaTime;

        //일정 시간 뒤 공격
        if (Cur_Delay >= Delay)
        {
            e_State = E_State.Attack ;
            Cur_Delay = 0;
            return;
        }
    }

    //플레이어에게 돌격
    void CrushPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, speed * Time.deltaTime);
        var dis = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);
        Debug.Log(dis);
        if (dis <= 3f)
        {
            //만약 플레이어가 공격 상태라면?
            var col = GameObject.Find("AttackPoint_Down").GetComponent<Collider2D>();
           var attackstate = col.enabled;

            //몬스터가 공격받는 상태
            if (attackstate)
            {
                e_State = E_State.Hit;
                //보스가 공격 받는거 추가
                monsterMaxHp--;
                Debug.Log(monsterMaxHp);
                if (monsterMaxHp <= 0)
                { 
                    Destroy(gameObject);
                    Debug.Log("몬스터 죽음");
                }

                return;
            }
            //유저 공격 받는 코드 추가
            GameManager.instance.player.Damage(Damage);
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
}