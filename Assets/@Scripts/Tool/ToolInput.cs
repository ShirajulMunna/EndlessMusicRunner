using System;
using System.Collections.Generic;
using UnityEngine;

public class ToolInput : MonoBehaviour, IToolInput
{
    public Dictionary<KeyCode, Action> D_Action { get; set; } = new Dictionary<KeyCode, Action>();

    private void Update()
    {
        UpdateInput();
    }

    public void SetInput(KeyCode code, Action action)
    {
        D_Action.TryAdd(code, null);
        D_Action[code] += action;
    }


    public void UpdateInput()
    {
        foreach (var item in D_Action)
        {
            if (!Input.GetKeyDown(item.Key))
            {
                continue;
            }
            item.Value?.Invoke();
        }
    }

}

interface IToolInput
{
    Dictionary<KeyCode, System.Action> D_Action { get; set; }
    void SetInput(KeyCode code, System.Action action);
    void UpdateInput();
}