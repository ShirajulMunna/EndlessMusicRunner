using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ToolDataManager : MonoBehaviour, IToolDataManager
{
    string path = Path.Combine(Application.streamingAssetsPath, "ToolData");
    public Dictionary<string, ToolData> D_Data { get; set; } = new Dictionary<string, ToolData>();

    bool isLoad;

    public void SetSave(string name, int bpm, List<double> timepoint)
    {
        var check = D_Data.TryGetValue(name, out var datas);

        datas = new ToolData(name, bpm, timepoint);
        D_Data.TryAdd(name, null);
        D_Data[name] = datas;

        var data = JsonUtility.ToJson(D_Data[name]);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var filePath = Path.Combine(path, D_Data[name].Name + ".dat");

        // 바이너리로 변환하여 저장
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(data);
            }
        }
    }

    // null 리턴해도 상관없음
    public ToolData GetLoad(string name)
    {
        if (!isLoad)
        {
            LoadAllData();
            isLoad = true;
        }

        D_Data.TryGetValue(name, out var data);
        return data;
    }

    void LoadAllData()
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        var files = Directory.GetFiles(path, "*.dat");
        foreach (var file in files)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    var jsonData = reader.ReadString();
                    var toolData = JsonUtility.FromJson<ToolData>(jsonData);
                    D_Data[toolData.Name] = toolData;
                }
            }
        }
    }
}

public interface IToolDataManager
{
    Dictionary<string, ToolData> D_Data { get; set; }
    void SetSave(string name, int bpm, List<double> timepoint);
    ToolData GetLoad(string name);
}

[Serializable]
public class ToolData
{
    public string Name;
    public int BPM;
    public List<double> L_TimePoint;

    public ToolData(string Name, int bpm, List<double> timepoint)
    {
        this.Name = Name;
        BPM = bpm;
        L_TimePoint = timepoint;
    }
}
