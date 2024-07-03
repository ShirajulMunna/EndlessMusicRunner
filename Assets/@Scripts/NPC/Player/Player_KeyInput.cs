using System;
using System.Collections.Generic;
using UnityEngine;

public class Player_KeyInput : MonoBehaviour, IKeyInput
{
    public Dictionary<KeyCode, System.Action> D_KeyInput_Down { get; set; } = new Dictionary<KeyCode, System.Action>();
    public Dictionary<KeyCode, System.Action> D_KeyInput_Up { get; set; } = new Dictionary<KeyCode, System.Action>();
    public Dictionary<(KeyCode, KeyCode), System.Action> D_TwinKeyInput { get; set; } = new Dictionary<(KeyCode, KeyCode), System.Action>();

    private float keyPressThreshold = 0.2f; // 키 입력 간격 허용 시간 (초)
    private Dictionary<KeyCode, float> keyPressTimes = new Dictionary<KeyCode, float>();

    private void Update()
    {
        foreach (var keyAction in D_KeyInput_Down)
        {
            if (Input.GetKeyDown(keyAction.Key))
            {
                keyAction.Value?.Invoke();
            }
        }

        foreach (var twinKeyAction in D_TwinKeyInput)
        {
            var key1 = twinKeyAction.Key.Item1;
            var key2 = twinKeyAction.Key.Item2;

            if (Input.GetKeyDown(key1))
            {
                keyPressTimes[key1] = Time.time;
            }

            if (Input.GetKeyDown(key2))
            {
                keyPressTimes[key2] = Time.time;
            }

            if (keyPressTimes.ContainsKey(key1) && keyPressTimes.ContainsKey(key2))
            {
                if (Mathf.Abs(keyPressTimes[key1] - keyPressTimes[key2]) <= keyPressThreshold)
                {
                    twinKeyAction.Value?.Invoke();
                    keyPressTimes.Remove(key1);
                    keyPressTimes.Remove(key2);
                }
            }
        }

        foreach (var keyAction in D_KeyInput_Up)
        {
            if (Input.GetKeyDown(keyAction.Key))
            {
                keyAction.Value?.Invoke();
            }
        }
    }

    public void AddKeyPoint_Down(KeyCode code, System.Action action)
    {
        if (!D_KeyInput_Down.ContainsKey(code))
        {
            D_KeyInput_Down[code] = null;
        }
        D_KeyInput_Down[code] += action;
    }

    public void RemoveKeyPoint_Down(KeyCode code, System.Action action)
    {
        if (D_KeyInput_Down.ContainsKey(code))
        {
            D_KeyInput_Down[code] -= action;
        }
    }

    public void AddTwinKeyPoint(KeyCode code1, KeyCode code2, System.Action action)
    {
        var keyPair = (code1, code2);
        if (!D_TwinKeyInput.ContainsKey(keyPair))
        {
            D_TwinKeyInput[keyPair] = null;
        }
        D_TwinKeyInput[keyPair] += action;
    }

    public void RemoveTwinKeyPoint(KeyCode code1, KeyCode code2, System.Action action)
    {
        var keyPair = (code1, code2);
        if (D_TwinKeyInput.ContainsKey(keyPair))
        {
            D_TwinKeyInput[keyPair] -= action;
        }
    }

    public void AddKeyPoint_Up(KeyCode code, Action action)
    {
        if (!D_KeyInput_Down.ContainsKey(code))
        {
            D_KeyInput_Down[code] = null;
        }
        D_KeyInput_Down[code] += action;
    }

    public void RemoveKeyPoint_Up(KeyCode code, Action action)
    {
        if (D_KeyInput_Up.ContainsKey(code))
        {
            D_KeyInput_Up[code] -= action;
        }
    }
}
