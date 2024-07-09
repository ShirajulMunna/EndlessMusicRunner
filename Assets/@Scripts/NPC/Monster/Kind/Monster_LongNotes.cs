using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_LongNotes : Monsters
{
    const string Name = "LongEffect_{0}";
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

    GameObject G_Effect;

    [SerializeField] SpriteRenderer[] myNoteSprite;
    float LongDestoryTime = 0.5f;
    float LongDestoryTime_POS = 0.1f;
    [SerializeField] int EffectIdx;

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
        float deltaTime = Time.deltaTime;
        LongDestoryTime -= deltaTime;
        LongDestoryTime_POS -= deltaTime;

        if (LongDestoryTime <= 0)
        {
            LongDestoryTime = 0;  // 음수가 되지 않도록 보장
            Destroy(this.gameObject);
            ActiveEndEffect();
        }

        // 점수 및 사운드 관련 로직
        if (GetScoreTime + 0.1f <= Time.time)
        {
            GetScoreTime = Time.time;
            var score = 1;
            ScoreManager.instance.SetCurrentScore(score);
        }

        SoundTime += deltaTime;
        if (SoundTime >= SoundTimeDuration)
        {
            SoundTime = 0f;
            AudioManager.instance.LongNoteSound();
        }

        nPC_Move.SetSpeed(0);

        // myNoteSprite[1]의 크기를 LongDestoryTime에 정확히 비례하여 조절
        float initialScaleX = 5f;  // 초기 X 스케일 값
        float scaleRatio = LongDestoryTime / initialDestoryTime;
        float newScaleX = initialScaleX * scaleRatio;
        Vector3 newScale = new Vector3(newScaleX, 1, 1);
        myNoteSprite[1].transform.localScale = newScale;

        // myNoteSprite[2]의 위치를 LongDestoryTime에 비례하여 15에서 0에 가까워지도록 이동
        float initialX = 15f;  // 초기 X 위치
        float newX = initialX * (LongDestoryTime_POS / initialDestoryTime);
        Vector3 sprite2Position = myNoteSprite[2].transform.position;
        sprite2Position.x = newX;
        myNoteSprite[2].transform.position = sprite2Position;
    }

    void ActiveEndEffect()
    {
        if (G_Effect == null)
        {
            return;
        }

        Destroy(G_Effect);
    }

    async void ActiveStartEffect()
    {
        var name = string.Format(Name, EffectIdx);
        G_Effect = await name.CreateOBJ<GameObject>();
        if (this.gameObject == null)
        {
            return;
        }
        G_Effect.transform.position = transform.position;
    }

    public override void SetHit(int hp)
    {
        ActiveStartEffect();
        isHold = true;
    }

    public override void SetUp(int hp, float speed, int damage, Vector3 target)
    {
        base.SetUp(hp, speed, damage, target);
    }

    public override void SetActive()
    {
        base.SetActive();
        initialDestoryTime = LongDestoryTime;
    }
}