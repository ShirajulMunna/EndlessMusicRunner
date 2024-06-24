using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DataSet :MonoBehaviour
{
    static readonly string StreamingBinary_Path = Application.streamingAssetsPath + "/Binary{0}.bin";
    static readonly string StreamingText_Path = Application.streamingAssetsPath + "/DataText{0}.txt";
    public static T ReadData_Sync<T>(string filename)
    {
        try
        {
            StreamReader sr = new StreamReader(string.Format(StreamingText_Path, filename));
            string datas = sr.ReadToEnd();
            sr.Close();
            T classdata = JsonUtility.FromJson<T>(datas);
            return classdata;
        }
        catch (Exception e)
        {
            Debug.LogError("ErrorFile:" + filename + "\n" + e);
            return default;
        }
    }
}
