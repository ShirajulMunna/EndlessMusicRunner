using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnTimePoint : MonoBehaviour, ISpawnTimePoint
{
    public ToolData toolData { get; set; }

    List<double> L_Times = new List<double>();
    ToolDataManager toolDataManager;

    public void SetUp(string name)
    {
        L_Times.Clear();
        toolDataManager = GetComponent<ToolDataManager>();
        toolData = toolDataManager.GetLoad(name);
        L_Times = toolData.L_TimePoint.ToList();
    }

    public bool CheckTime(double times)
    {
        if (L_Times.Count <= 0)
        {
            return false;
        }

        return times > L_Times[0];
    }

    public void RemoveTimes()
    {
        L_Times.RemoveAt(0);
    }

    public bool CheckEndTiems()
    {
        return L_Times.Count <= 0;
    }
}

interface ISpawnTimePoint
{
    ToolData toolData { get; set; }
    void SetUp(string name);
    bool CheckTime(double times);
    void RemoveTimes();
    bool CheckEndTiems();
}