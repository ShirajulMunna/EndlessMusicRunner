using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // 이벤트 처리를 위한 네임스페이스 추가
using TMPro;

public class UI_ToolInputTimeLine : MonoBehaviour, IPointerClickHandler // 인터페이스 추가
{
    [SerializeField] TMP_InputField tMP_InputField;
    double Times;
    ToolTimePoint toolTimepoint;

    public void SetUp(double times, ToolTimePoint toolTimePoint)
    {
        Times = times;
        toolTimepoint = toolTimePoint;
        tMP_InputField.text = Times.ToString();
    }

    public void Sync()
    {
        Times = double.Parse(tMP_InputField.text);
    }

    public double GetTimes()
    {
        return Times;
    }

    public void Btn_Destory()
    {
        toolTimepoint.RemovePoint(this);
        Destroy(this.gameObject);
    }

    // IPointerClickHandler 인터페이스 구현
    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 오른쪽 버튼을 체크합니다 (우클릭)
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Btn_Destory();
        }
    }
}