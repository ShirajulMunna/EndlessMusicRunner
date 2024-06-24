using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    [SerializeField] TMP_InputField Input_Name;
    [SerializeField] TMP_InputField Input_Bpm;

    ToolSlide toolSlide;
    ToolInput toolInput;
    ToolAudio toolAudio;
    ToolTimePoint toolTimePoint;
    ToolDataManager toolDataManager;
    ToolPlayer toolPlayer;

    bool isLoad;

    private void Start()
    {
        SetToolinput(GetComponent<ToolInput>());
        SetSlider(GetComponent<ToolSlide>());
        SetAudio(GetComponent<ToolAudio>());
        SetTimePoint(GetComponent<ToolTimePoint>());
        toolDataManager = GetComponent<ToolDataManager>();
        toolPlayer = GetComponent<ToolPlayer>();
    }

    private void FixedUpdate()
    {
        HandleMouseWheel();
    }

    #region 셋팅

    public void SetToolinput(ToolInput toolinput)
    {
        toolInput = toolinput;
    }

    void SetSlider(ToolSlide toolslide)
    {
        toolSlide = toolslide;
    }

    void SetAudio(ToolAudio toolAudio)
    {
        this.toolAudio = toolAudio;
    }

    void SetTimePoint(ToolTimePoint toolTimepoint)
    {
        toolTimePoint = toolTimepoint;
    }
    #endregion

    #region 휠조작

    // 마우스 휠 입력을 처리하는 메서드
    private void HandleMouseWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            toolSlide?.UpdateSlider(scroll);
            toolAudio?.Pause();
            toolAudio?.UpdateAudioTime_Scroll();
        }
    }

    #endregion

    #region 버튼

    public void Btn_Load()
    {
        isLoad = true;
        var max = toolAudio.SetAudioRange();
        toolSlide?.SetSlider_Value(max);
    }

    public void Btn_Play()
    {
        if (!isLoad)
        {
            return;
        }

        toolAudio?.Play();
        toolPlayer?.Player();
    }

    public void Btn_Pause()
    {
        toolAudio.Pause();
    }

    public void Btn_DataSave()
    {
        toolTimePoint.SaveAddPoint();
        toolDataManager.SetSave(Input_Name.text, int.Parse(Input_Bpm.text), toolTimePoint.GetPoint());
    }
    public void Btn_DataLoad()
    {
        toolTimePoint.Reset();
        var data = toolDataManager.GetLoad(Input_Name.text);

        if (data == null)
        {
            return;
        }

        Input_Name.text = data.Name;
        Input_Bpm.text = data.BPM.ToString();
        foreach (var item in data.L_TimePoint)
        {
            toolTimePoint.AddPoint(item);
        }
        toolTimePoint.SaveAddPoint();
    }

    //로드했는지 체크
    public bool CheckLoad()
    {
        return isLoad;
    }
    #endregion
}