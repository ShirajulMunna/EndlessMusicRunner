using Spine.Unity;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class LongNote : MonoBehaviour
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

    GameObject Effect;
    float Dealy = 0f;

    private float GetScoreTime = 0f;

    public int AttackHold = 0;
    //�ճ�Ʈ �����
    public static void Create(string folderName, string name, Vector3 CreatePos, int speed)
    {
        string path = $"{folderName}/{name}";
        var load = Resources.Load<GameObject>(path);
        var note = Instantiate<GameObject>(load);
        note.transform.position = CreatePos;
        //note.GetComponent<Monster>().Speed = speed;
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
    }

    private void Update()
    {
        SetCheck();
    }

    void SetCheck()
    {
        if (AttackHold == 0 || AttackHold == 2)
        {
            // ���� ���߿� ���߰ų� ���������� 
            var targetPosition = GameManager.instance.player.transform.position;
            if (transform.position.x <= targetPosition.x)
            {
                GameManager.instance.player.SetHp(-5);
                ScoreManager.instance.SetBestCombo_Reset();
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

            //스케일 줄이기
            var scale = Tr.localScale;
            scale.x -= Scale_X;

            var pos = myNoteSprite[1].transform.position;
            pos.x += Star_X;
            myNoteSprite[1].transform.position = pos;

            Tr.localScale = scale;
            //
            return;
        }
        AttackHold = 2;
        if (Effect)
        {
            Destroy(Effect);
        }
    }

    public void SetAttack()
    {
        if (AttackHold == 0)
        {
            AttackHold = 1;
            ScoreManager.instance.SetCombo_Add(); // �޺��߰�
            return;
        }

        if (AttackHold == 2)
        {
            return;
        }

        if (Effect == null)
        {
            var createposr = GameManager.instance.lowerAttackPoint.transform.position;
            Effect = Instantiate(G_Effect, createposr, default, null);
        }

        var scale = Tr.localScale;
        scale.x -= Scale_X;

        var pos = myNoteSprite[1].transform.position;
        pos.x += Star_X;
        myNoteSprite[1].transform.position = pos;

        Tr.localScale = scale;

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
        Destroy(Effect);
        var createpos = GameManager.instance.skeleton.transform.position;
        createpos.x += 1;
        createpos.y = 0;
        var end = Instantiate(G_End, createpos, default, null);
        Destroy(end, 1f);

    }
}