using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_LongNotes : Monsters
{

    Player_Attacker player_Attacker
    {
        get => GameManager.M_Player.player_Attacker;
    }

    NPC player
    {
        get => GameManager.M_Player;
    }

    float GetScoreTime;
    float SoundTime;
    float SoundTimeDuration = 0.15f;
    bool isHold = false;
    float initialDestoryTime;
    Vector3 initialPosition;

    [SerializeField] SpriteRenderer[] myNoteSprite;
    [SerializeField] float DestoryTime;
    [SerializeField] GameObject G_StartEffect;
    [SerializeField] GameObject G_EndEffect;




    void Start()
    {
        initialDestoryTime = DestoryTime;
        initialPosition = myNoteSprite[2].transform.position;
    }

    private void Update()
    {
        if (!isHold)
        {
            return;
        }

        if (!player_Attacker.CheckHold())
        {
            SetAttack(player);
            Destroy(this.gameObject);
            ActiveEndEffect();
            return;
        }

        UpdateScale();
    }

    void UpdateScale()
    {
        DestoryTime -= Time.deltaTime;

        if (DestoryTime <= 0)
        {
            Destroy(this.gameObject);
            return;
        }

        // 점수 및 사운드 관련 로직
        if (GetScoreTime + 0.1f <= Time.time)
        {
            GetScoreTime = Time.time;
            var score = 1;
            ScoreManager.instance.SetCurrentScore(score);
        }

        SoundTime += Time.deltaTime;
        if (SoundTime >= SoundTimeDuration)
        {
            SoundTime = 0f;
            AudioManager.instance.LongNoteSound();
        }

        nPC_Move.SetSpeed(0);

        // myNoteSprite[1]의 크기를 DestoryTime에 비례하여 조절
        float scaleRatio = DestoryTime / initialDestoryTime; // initialDestoryTime은 DestoryTime의 초기값
        Vector3 newScale = new Vector3(scaleRatio, 1, 1); // Y와 Z 스케일은 변경하지 않음
        myNoteSprite[1].transform.localScale = newScale;

        // myNoteSprite[1]의 위치 조정 (선택적)
        Vector3 newPosition = myNoteSprite[2].transform.position;
        newPosition.x = initialPosition.x + (1 - scaleRatio) * (initialPosition.x - transform.position.x);
        myNoteSprite[2].transform.position = newPosition;
    }

    void ActiveEndEffect()
    {
        G_EndEffect.SetActive(true);
        G_EndEffect.transform.SetParent(null);
        Destroy(G_EndEffect, 1f);
        Destroy(G_StartEffect, 1f);
    }

    void ActiveStartEffect()
    {
        G_StartEffect.SetActive(true);
        G_StartEffect.transform.SetParent(null);
    }

    public override void SetHit(int hp)
    {
        base.SetHit(hp);
        ActiveStartEffect();
        isHold = true;
    }
}