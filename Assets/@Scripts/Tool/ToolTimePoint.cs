using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTimePoint : MonoBehaviour, ITimePointManager, ITimePointCreator
{
    [SerializeField] private GameObject G_TimePoints;
    [SerializeField] private Transform Tr_Creates;

    public GameObject G_TimePoint { get; set; }
    public Transform Tr_Create { get; set; }
    public List<UI_ToolInputTimeLine> L_InputList { get; set; } = new List<UI_ToolInputTimeLine>();

    private ToolAudio toolAudio;
    private ToolInput toolInput;
    private ToolManager toolManager;

    private void Awake()
    {
        toolAudio = GetComponent<ToolAudio>();
        toolInput = GetComponent<ToolInput>();
        toolManager = GetComponent<ToolManager>();
        G_TimePoint = G_TimePoints;
        Tr_Create = Tr_Creates;
    }

    private void Start()
    {
        AddTimePoint();
    }

    public List<UI_ToolInputTimeLine> GetPoint()
    {
        return L_InputList;
    }

    public void AddPoint(double times)
    {
        var point = CreateTimePoint(times);
        L_InputList.Add(point);
    }

    public void Reset()
    {
        foreach (var item in L_InputList)
        {
            Destroy(item.gameObject);
        }
        L_InputList.Clear();
    }

    public UI_ToolInputTimeLine CreateTimePoint(double times)
    {
        var point = Instantiate(G_TimePoint, Tr_Create).GetComponent<UI_ToolInputTimeLine>();
        point.SetUp(times, this);
        return point;
    }

    public void AddTimePoint()
    {
        System.Action action = () =>
        {
            var check = toolManager.CheckLoad();
            if (!check)
            {
                return;
            }
            var times = toolAudio.GetAudioTime();
            AddPoint(times);
        };

        toolInput.SetInput(KeyCode.Space, action);
    }

    public void RemovePoint(UI_ToolInputTimeLine line)
    {
        L_InputList.Remove(line);
    }
}

public interface ITimePointCreator
{
    GameObject G_TimePoint { get; set; }
    Transform Tr_Create { get; set; }
    UI_ToolInputTimeLine CreateTimePoint(double times);
}

public interface ITimePointManager
{
    List<UI_ToolInputTimeLine> GetPoint();
    void AddPoint(double times);
    void AddTimePoint();
    void Reset();
}
