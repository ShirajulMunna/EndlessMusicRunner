using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToolPlayer : MonoBehaviour, IToolPlayer
{
    public List<UI_ToolInputTimeLine> L_TimePoint { get; set; }
    ToolTimePoint toolTimePoint;
    ToolEffect toolEffect;
    ToolAudio toolAudio;

    private void Awake()
    {
        toolEffect = GetComponent<ToolEffect>();
        toolTimePoint = GetComponent<ToolTimePoint>();
        toolAudio = GetComponent<ToolAudio>();
    }

    private void FixedUpdate()
    {
        if (!toolAudio.CheckPlay())
        {
            return;
        }
        var curTime = toolAudio.GetAudioTime();
        UpdatePlayer(curTime);
    }

    public void Player()
    {
        L_TimePoint = toolTimePoint.GetPoint().ToList();
    }

    public void UpdatePlayer(double curtime)
    {
        if (L_TimePoint == null)
        {
            return;
        }

        if (L_TimePoint.Count <= 0)
        {
            return;
        }

        var check = curtime < L_TimePoint[0].GetTimes();

        if (check)
        {
            return;
        }

        L_TimePoint.RemoveAt(0);
        toolEffect.CreateEffect();
    }
}

interface IToolPlayer
{
    List<UI_ToolInputTimeLine> L_TimePoint { get; set; }
    void Player();
    void UpdatePlayer(double curtime);
}