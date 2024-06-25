using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolAudio : MonoBehaviour, IToolAudio
{

    [SerializeField] TextMeshProUGUI T_AudioTime;

    public AudioSource audioSource { get; set; }
    ToolManager toolManager;
    ToolSlide toolSlide;

    private void Awake()
    {
        toolManager = GetComponent<ToolManager>();
        toolSlide = GetComponent<ToolSlide>();
        SetAudio(GetComponent<AudioSource>());
    }

    private void FixedUpdate()
    {
        UpdateAudioTime();
    }

    public void SetAudio(AudioSource audio)
    {
        audioSource = audio;
    }

    public float SetAudioRange()
    {
        return audioSource.clip.length;
    }

    public void SetAudioTime(float value)
    {
        audioSource.time = value;
    }

    public float GetAudioTime()
    {
        return audioSource.time;
    }

    public bool CheckPlay()
    {
        return audioSource.isPlaying;
    }

    //오디오 시간
    public void UpdateAudioTime()
    {
        if (!toolManager.CheckLoad())
        {
            return;
        }

        T_AudioTime.text = GetAudioTime().ToString();
    }

    //스크롤 이동
    public void UpdateAudioTime_Scroll()
    {
        var times = toolSlide.GetSliderValue();
        if (times <= 0)
        {
            return;
        }
        SetAudioTime(times);
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Pause()
    {
        audioSource.Pause();
    }
}

interface IToolAudio
{
    AudioSource audioSource { get; set; }
    void SetAudio(AudioSource audio);
    float SetAudioRange();
    void Play();
    void Pause();
    void UpdateAudioTime_Scroll();
    void UpdateAudioTime();
    bool CheckPlay();
    float GetAudioTime();
    void SetAudioTime(float times);
}