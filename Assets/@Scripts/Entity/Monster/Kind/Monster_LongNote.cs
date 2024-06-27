using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class Monster_LongNote : Monster
{
    [SerializeField] Transform Tr;
    [SerializeField] GameObject G_Effect;
    [SerializeField] GameObject G_End;
    public SpriteRenderer[] myNoteSprite;
    public SpriteRenderer myLongSprrite;
    [SerializeField] Sprite[] noteSprites;
    [SerializeField] Sprite[] longSprites;
    [SerializeField] Vector3 BoxSize;
    [SerializeField] LayerMask layerMask;

    [SerializeField] float Star_X;
    [SerializeField] float Scale_X;

    GameObject effect;

    private float GetScoreTime = 0f;

    private int AttackHold = 0;

    private Vector3 prevPosition; //충돌지점에서 포지션고정
    private bool isAttackPlayer = false;

    public GameObject perfectEffect;
    public GameObject greatEffect;

    float SoundTime = 0f;
    public float SoundTimeDuration = 0.15f;

    System.Action Ac_Close;

    protected override void Start()
    {
        if (Change)
        {
            for (int i = 0; i < myNoteSprite.Length; ++i)
            {
                var idx = (int)PlayerSkinType.Count;
                // 두가지 타입으로 나눠져서 됨. type필요없음
                int spriteIndex = (int)UI_Lobby.playerSkinType % idx;
                myNoteSprite[i].sprite = noteSprites[spriteIndex];
                //����� 1�γ��ͼ� 0.5�� �������� ����
                myNoteSprite[i].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
        DestoryX = -50;
    }

    protected override void Update()
    {
        SetCheck();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    void SetCheck()
    {
        if (AttackHold == 0 || AttackHold == 2)
        {
            var targetPosition = GameManager.instance.player.transform.position;
            if (transform.position.x <= targetPosition.x)
            {
                isAttackPlayer = true;
                Destroy(gameObject);
            }
            return;
        }

        Ac_Close?.Invoke();

        var player = GameManager.instance.player;
        var check = player.M_Attack.GetAttackState(E_AttackState.Hold);

        if (check)
        {
            //딜레이 시간
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

            transform.position = prevPosition;

            //스케일 줄이기
            var scale = Tr.localScale;
            scale.x -= Scale_X;

            var pos = myNoteSprite[1].transform.position;
            pos.x -= Star_X;
            myNoteSprite[1].transform.position = pos;

            Tr.localScale = scale;
            return;
        }
        AttackHold = 2;
        if (effect)
        {
            //중간홀드하다가 끊길때
            isAttackPlayer = true;
            Destroy(effect);
            Destroy(gameObject);
        }
    }

    public void SetAttack(ScoreManager.E_ScoreState perfect)
    {
        AttackHold = 1;
        prevPosition = transform.position;
        GameManager.instance.longNoteDestoryPosition = prevPosition;
        ScoreManager.instance.SetCombo_Add(); // �޺��߰�
        ScoreManager.instance.SetScoreState(perfect);
        SetConditionEffect(perfect, prevPosition);
        AudioManager.instance.LongNoteSound();

        if (effect == null)
        {
            var createposr = /*GameManager.instance.lowerAttackPoint.*/transform.position;
            effect = Instantiate(G_Effect, createposr, default, null);
        }

        if (AttackHold == 2)
        {
            return;
        }

        System.Action action = () =>
        {
            if (Tr.localScale.x > 0)
            {
                return;
            }
            AudioManager.instance.LongNoteSound();
            ScoreManager.instance.SetCombo_Add();
            ScoreManager.instance.SetScoreState(perfect); //롱노트 이펙트는 추가되지만 정확한 내용이 들어가 있지 않음
                                                          //게임매니저에서 처음 충돌위치가져온상태
            var createpos = GameManager.instance.longNoteDestoryPosition;
            var end = Instantiate(G_End, createpos, default, null);
            SetConditionEffect(perfect, createpos);
            Destroy(end, 1f);
            Destroy(this.gameObject);
            Destroy(effect);
            Ac_Close = null;
        };
        Ac_Close = action;
    }

    public override void SetMove()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
        var values = DestoryX;
        if (transform.position.x < values)
        {
            isAttackPlayer = true;
            Destroy(this.gameObject);
        }
    }
    private async void OnDestroy()
    {
        if (isAttackPlayer)
        {
            // 파괴되면서 플레이어 데미지 및 이펙트 출력
            await HandleDestroyAsync();
        }
    }
    private async Task HandleDestroyAsync()
    {
        var playerPos = GameManager.instance.player.transform.position;
        if (playerPos.y + 0.5f >= transform.position.y && playerPos.y - 0.5f <= transform.position.y || uniqMonster == UniqMonster.SendBack)
        {
            GameManager.instance.player.SetHp(-5);
            ScoreManager.instance.SetBestCombo_Reset();
            ScoreManager.instance.SetScoreState(ScoreManager.E_ScoreState.Miss); // 롱노트 중간에 실패하면 Miss를 추가해주기

            var effects = await Effect.Create(transform.position, (int)HitCollisionDetection.ConditionEffect.Opps);
            effects.fadeDuration = HitCollisionDetection.Instance.fadeDuration;
        }
    }
    //판정이펙트 출력
    private void SetConditionEffect(ScoreManager.E_ScoreState perfect, Vector3 position)
    {
        GameObject effectObject = null;
        switch (perfect)
        {
            case ScoreManager.E_ScoreState.Perfect:
                effectObject = Instantiate(perfectEffect, position, default, null);
                break;
            case ScoreManager.E_ScoreState.Great:
                effectObject = Instantiate(greatEffect, position, default, null);
                break;
        }
        if (effectObject != null)
            effectObject.GetComponent<Effect>().fadeDuration = HitCollisionDetection.Instance.fadeDuration;
    }
}