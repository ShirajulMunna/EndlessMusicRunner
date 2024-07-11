using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnTimePoint : MonoBehaviour, ISpawnTimePoint
{
    public ToolData toolData { get; set; }
    public SpawnDelay spawnDelay { get; set; }

    List<double> L_Times = new List<double>();
    ToolDataManager toolDataManager;

    float DelayEnd;

    public void SetUp(string name)
    {
        L_Times.Clear();
        spawnDelay = GetComponent<SpawnDelay>();
        toolDataManager = GetComponent<ToolDataManager>();
        toolData = toolDataManager.GetLoad(name);
        L_Times = toolData.L_TimePoint.ToList();
        DelayEnd = 4;
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
        var isend = L_Times.Count <= 0;
        if (!isend)
        {
            return false;
        }

        DelayEnd -= Time.deltaTime;
        if (DelayEnd > 0)
        {
            return false;
        }

        return true;
    }
}
