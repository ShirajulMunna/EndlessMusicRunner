using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTimePoint : MonoBehaviour, ITimePointManager, ITimePointCreator
{
    [SerializeField] private GameObject G_TimePoints;
    [SerializeField] private Transform Tr_Creates;

    public GameObject G_TimePoint { get; set; }
    public Transform Tr_Create { get; set; }
    public List<double> L_TimePoint { get; set; } = new List<double>();
    public List<TMP_InputField> L_InputList { get; set; } = new List<TMP_InputField>();

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

    public List<double> GetPoint()
    {
        return L_TimePoint;
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
            Destroy(item);
        }
        L_InputList.Clear();
    }

    //저장
    public void SaveAddPoint()
    {
        L_TimePoint.Clear();
        foreach (var item in L_InputList)
        {
            var times = double.Parse(item.text);
            L_TimePoint.Add(times);
        }
    }

    public TMP_InputField CreateTimePoint(double times)
    {
        var point = Instantiate(G_TimePoint, Tr_Create).GetComponent<TMP_InputField>();
        point.text = times.ToString();
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
}

public interface ITimePointCreator
{
    GameObject G_TimePoint { get; set; }
    Transform Tr_Create { get; set; }
    TMP_InputField CreateTimePoint(double times);
}

public interface ITimePointManager
{
    List<double> GetPoint();
    void AddPoint(double times);
    void AddTimePoint();
    void SaveAddPoint();
    void Reset();
}
