using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class ToolSlide : MonoBehaviour, IToolSlider
{
    [SerializeField] Slider _slider;
    public Slider Slider { get; set; }

    ToolAudio toolAudio;

    private void Awake()
    {
        toolAudio = GetComponent<ToolAudio>();
        SetSlider(_slider);
    }

    private void FixedUpdate()
    {
        UpdateSlider();
    }

    //슬라이더 업데이트
    public void UpdateSlider()
    {
        if (!toolAudio.CheckPlay())
        {
            return;
        }

        var time = toolAudio.GetAudioTime();
        SetSyncSliderInMusic(time);
    }

    public void SetSlider(Slider slider)
    {
        Slider = slider;
    }

    public void SetSlider_Value(float value)
    {
        Slider.maxValue = value;
        UpdateSlider(0);
    }

    public void UpdateSlider(float value)
    {
        Slider.value += value;
    }

    public void SetSyncSliderInMusic(float time)
    {
        Slider.value = time;
    }

    public float GetSliderValue()
    {
        return Slider.value;
    }
}

interface IToolSlider
{
    Slider Slider { get; set; }
    void SetSlider(Slider slider);
    void SetSlider_Value(float value);
    void UpdateSlider(float value);
    float GetSliderValue();
    void SetSyncSliderInMusic(float time);
}