using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    public SkeletonAnimation skeleton;
    public GameObject speaker_1;
    public GameObject speaker_2;


    public PlayerSystem player;
    public Transform bossWaitPosition;

    public Transform lowerAttackPoint;
    [HideInInspector] public Vector3 longNoteDestoryPosition;

    private Vector3 GameResultPosition = new Vector3(10, -2, 0);
    void Awake()
    {
        Application.targetFrameRate = 120; //120���Τ� ����������
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        skeleton = GameObject.Find("Player").transform.GetChild(0).GetComponent<SkeletonAnimation>();
        Application.targetFrameRate = 120;
    }

    // After playing some times background will
    // will change autometically .
    // Implement it here

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speaker_1.gameObject.SetActive(true);
            speaker_2.gameObject.SetActive(true);
            Invoke("DeactivatSpeaker", 5f);

        }
    }
    //���� ��� ��������
    public void SetGameResult(GameResultType type)
    {
        Spine_GameResult.Create(GameResultPosition, type);
    }
}