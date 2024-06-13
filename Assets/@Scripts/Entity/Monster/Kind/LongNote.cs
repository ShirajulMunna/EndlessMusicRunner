using UnityEngine;
using System.Threading.Tasks;

public class LongNote : MonoBehaviour, IMonsterMove
{
    [SerializeField] Transform Tr;
    [SerializeField] GameObject G_Effect;
    [SerializeField] GameObject G_End;
    [SerializeField] Transform start_1;
    [SerializeField] Transform start_2;
    [SerializeField] GameObject lowecollision;

    public SpriteRenderer[] myNoteSprite;
    public SpriteRenderer myLongSprrite;
    [SerializeField] bool Change;
    [SerializeField] Sprite[] noteSprites;
    [SerializeField] Sprite[] longSprites;
    [SerializeField] Vector3 BoxSize;
    [SerializeField] LayerMask layerMask;

    [SerializeField] float Star_X;
    [SerializeField] float Scale_X;

    GameObject effect;
    float Dealy = 0f;

    private float GetScoreTime = 0f;

    public int AttackHold = 0;

    public float Speed {get ; set ;}
    public float DestoryX { get; set; }

    private Vector3 prevPosition; //충돌지점에서 포지션고정
    private bool isAttackPlayer = false;

    //�ճ�Ʈ �����
    public static void Create(string folderName, string name, Vector3 CreatePos, int speed)
    {
        string path = $"{folderName}/{name}";
        var load = Resources.Load<GameObject>(path);
        var note = Instantiate<GameObject>(load);
        note.transform.position = CreatePos;
        var noteValue = note.GetComponent<LongNote>();
        noteValue.Speed = (float)speed;
    }
    private void Start()
    {
        if (Change)
        {
            var type = UI_Lobby.Type == false ? 0 : 1;
            // �̹��� ���� �߰�
            for (int i = 0; i < myNoteSprite.Length; ++i)
            {
                var idx = (int)PlayerSkinType.Count;
                int spriteIndex = (int)UI_Lobby.playerSkinType % idx;
                if (type == 1)
                    spriteIndex += (int)PlayerSkinType.Count;

                myNoteSprite[i].sprite = noteSprites[spriteIndex];
                //����� 1�γ��ͼ� 0.5�� �������� ����
                myNoteSprite[i].gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            myLongSprrite.sprite = longSprites[type];
        }
        DestoryX = -50;
    }

    private void Update()
    {
        SetMove();
        SetCheck();
    }

    async void SetCheck()
    {
        if (AttackHold == 0 || AttackHold == 2)
        {
            // ���� ���߿� ���߰ų� ���������� 
            var targetPosition = GameManager.instance.player.transform.position;
            if (transform.position.x <= targetPosition.x)
            {
                isAttackPlayer=true;
                Destroy(gameObject);
            }
            return;
        }

        var player = GameManager.instance.player;
        if (player.M_Attack.GetAttackState(E_AttackState.Hold))
        {
            //딜레이 시간
            if (GetScoreTime + 0.1f <= Time.time)
            {
                GetScoreTime = Time.time;
                var score = 1;
                Debug.Log(score);
                ScoreManager.instance.SetCurrentScore(score);
            }
            transform.position = prevPosition;
            //스케일 줄이기
            var scale = Tr.localScale;
            scale.x -= Scale_X;

            var pos = myNoteSprite[1].transform.position;
            pos.x -= Star_X;
            myNoteSprite[1].transform.position = pos;

            Tr.localScale = scale;
            //
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

    public void SetAttack()
    {
        if (AttackHold == 0)
        {
            AttackHold = 1;
            prevPosition = transform.position;
            ScoreManager.instance.SetCombo_Add(); // �޺��߰�
            return;
        }

        if (AttackHold == 2)
        {
            return;
        }

        if (effect == null)
        {
            var createposr = GameManager.instance.lowerAttackPoint.transform.position;
            effect = Instantiate(G_Effect, createposr, default, null);
        }

        Dealy -= Time.deltaTime;

        if (Dealy > 0)
        {
            AudioManager.instance.PlaySound();
            Dealy = 0.1f;
            AttackHold = 1;
        }



        if (Tr.localScale.x > 0)
        {
            return;
        }

        ScoreManager.instance.SetCombo_Add(); // �޺��߰�
        Destroy(this.gameObject);
        Destroy(effect);
        var createpos = GameManager.instance.skeleton.transform.position;
        createpos.x += 1;
        createpos.y = 0;
        var end = Instantiate(G_End, createpos, default, null);
        Destroy(end, 1f);

    }

    public void SetMove()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
        var values = DestoryX;
        if (transform.position.x < values)
        {
            isAttackPlayer=true;
            Destroy(this.gameObject);
        }
    }
    private async void OnDestroy()
    {
        if(isAttackPlayer)
        {
            // 파괴되면서 플레이어 데미지 및 이펙트 출력
            await HandleDestroyAsync();
        }
    }
    private async Task HandleDestroyAsync()
    {
        GameManager.instance.player.SetHp(-5);
        ScoreManager.instance.SetBestCombo_Reset();

        var effects = await Effect.Create(transform.position, (int)HitCollisionDetection.ConditionEffect.Opps);
        effects.fadeDuration = HitCollisionDetection.Instance.fadeDuration;
    }
}