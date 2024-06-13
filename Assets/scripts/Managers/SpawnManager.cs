using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    bool isCompletedSpawn = false;
    float gameOverTime = 0f;
    public int StageInfo = 0; //추후 데이터 어디에서 데이터 받아오는 형태로 만들예정
    public enum E_SpawnPoint
    {
        Low,
        Middle,
        Hight,
        BossPoint,
    }

   List<Vector3> L_SpawnPoint = new List<Vector3>()
    {
        new Vector3(20, -3.5f, 0),
        new Vector3(20, 0, 0),
        new Vector3(20, 3.5f, 0),
       new Vector3(12, -2.5f, 0),
    };

    public List<GameObject> monsterOBjects = new List<GameObject>();
    public List<GameObject> bossObjects = new List<GameObject>();
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);

        }
        else
        {
            instance = this;
        }

    }

    private void Update()
    {
        if (isCompletedSpawn)
        {
            gameOverTime += Time.deltaTime;

            if (gameOverTime >= 2.5f)
            {
                AudioManager.instance.StopMusic();
                UI_GameOver.Create();
                isCompletedSpawn = false;
            }
        }
    }

    public void StartSpawningObjects(bool isSpawn)
    {
        if (isSpawn)
            StartCoroutine(SpawnMonstersAtRandomPos());
    }
   
    IEnumerator SpawnMonstersAtRandomPos()
    {
        //몬스터 오브젝트풀링으로 추후 변경예정
        int counting = 0;
        bool isNewCreatBoss = false; // 보스생성했는가에 대한 변수
        Boss boss = null;
        var audio = AudioManager.instance;
        while (true)
        {
            var level = GameData.Data.LevelDesigin[StageInfo];
            int idx = 0;
            for (int i = 0; i < level.Count; i++)
            {
                var monsterInfo = GameData.Data.MonsterTable[level[idx].MonsterInfo];
                if(isNewCreatBoss)
                {
                    
                    audio.Audio_BackGround.PlayOneShot(audio.bossAttackClip);

                    if(boss ==null)
                        boss = GameObject.FindAnyObjectByType<Boss>();
                }
                if (boss != null)
                    boss.PlayerAttack("Attack1", true);
                if (level[idx].MonsterInfo / 1000 == 1)
                {
                    for (int j = 0; j < level[idx].MonsterSpwanCount; ++j)
                    {
                        LongNoteSpawn(monsterInfo, (MonsterSpwanPosition)level[idx].Spwan_Position);

                        yield return new WaitForSeconds(level[idx].CoolTime);
                    }
                }
                else
                {
                    for (int j = 0; j < level[idx].MonsterSpwanCount; ++j)
                    {
                        //보스 한번 생성했는지검사
                        bool isBoss = monsterInfo.monsterType == Monster_Type.Boss;
                        //보스 한번생성한적이 있다면 보스생성하지않고 돌아오는형태
                        if (isBoss && isNewCreatBoss) continue;

                        MonsterSpawn(monsterInfo, (MonsterSpwanPosition)level[idx].Spwan_Position);

                        if (isBoss)
                            isNewCreatBoss = true;
                        yield return new WaitForSeconds(level[idx].CoolTime);
                    }
                }
                idx++;
            }
            //보스 포지션은 테스트후 재설정 할예정
            var bossPosition = Vector3.zero;
            bossPosition.x = -40;

            counting++;
            if (counting == 2)
            {
                isCompletedSpawn = true;
                yield break;
            }
        }

    }
    
    public void MonsterSpawn(C_MonsterTable t, MonsterSpwanPosition spwanPosition, bool CreatBoss = false)
    {
        string prefab = string.Empty;
        bool isMonster = t.monsterType == Monster_Type.Normal;
        if (t.monsterType == Monster_Type.Normal)
            prefab = monsterOBjects[t.PrefabName].name;
        else if (t.monsterType == Monster_Type.Boss)
            prefab = bossObjects[t.PrefabName].name;

        if (t.Uniq_MonsterType == UniqMonster.SendBack)
        {
            Monster.Create("Monster", prefab, L_SpawnPoint[(int)E_SpawnPoint.Middle], t.MaxHp, t.Speed, t.Uniq_MonsterType);
            return;
        }
        var MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Hight];
        switch (spwanPosition)
        {
            case MonsterSpwanPosition.Down:
                MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                break;
            case MonsterSpwanPosition.Random:
                int random = Random.Range(0, 2);
                if (random == 1)
                    MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                break;
        }
        if (isMonster)
            Monster.Create("Monster", prefab, MySpwanPoint, t.MaxHp, t.Speed, t.Uniq_MonsterType);
        else if (!isMonster)
        {
            Boss.Create(L_SpawnPoint[(int)E_SpawnPoint.BossPoint]);
        }
    }

    public void LongNoteSpawn(C_MonsterTable t, MonsterSpwanPosition spwanPosition)
    {
        string prefab = string.Empty;
        prefab = monsterOBjects[t.PrefabName].name;

        var MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Hight];
        switch (spwanPosition)
        {
            case MonsterSpwanPosition.Down:
                MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                break;
            case MonsterSpwanPosition.Random:
                int random = Random.Range(0, 2);
                if (random == 1)
                    MySpwanPoint = L_SpawnPoint[(int)E_SpawnPoint.Low];
                break;
        }
        LongNote.Create("Monster", prefab, MySpwanPoint, t.Speed);
    }
}

