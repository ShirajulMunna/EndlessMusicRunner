using System;
using System.Collections.Generic;
using UnityEngine;

public enum E_ActionScene
{
    Lobby,
    Play,
}

public class ActionManager : Singleton<ActionManager>, IAction
{
    public Dictionary<(E_ActionScene, E_ActionList), System.Action> D_Action { get; set; } = new Dictionary<(E_ActionScene, E_ActionList), System.Action>();
    public List<E_ActionScene> list { get; set; } = new List<E_ActionScene>();

    static Dictionary<(E_ActionScene, E_ActionList), System.Action> D_Action_Static { get; set; } = new Dictionary<(E_ActionScene, E_ActionList), System.Action>();

    private void Start()
    {
        foreach (var item in Enum.GetValues(typeof(E_ActionScene)))
        {
            list.Add((E_ActionScene)item);
        }
    }

    private void Update()
    {
        foreach (var item in list)
        {
            SetAction((item, E_ActionList.Update));
        }
        foreach (var item in list)
        {
            SetAction((item, E_ActionList.Update));
        }
    }

    public void SetAction((E_ActionScene, E_ActionList) key)
    {
        if (D_Action.ContainsKey(key))
        {
            D_Action[key]?.Invoke();
        }
        if (D_Action_Static.ContainsKey(key))
        {
            D_Action_Static[key]?.Invoke();
        }
    }

    public void AddAction((E_ActionScene, E_ActionList) key, System.Action action, bool isstatic = false)
    {
        RemoveAction(key, action);
        if (!D_Action.ContainsKey(key))
        {
            D_Action[key] = null;
            D_Action_Static[key] = null;
        }
        if (isstatic)
        {
            D_Action_Static[key] += action;
        }
        else
        {
            D_Action[key] += action;
        }
    }

    public void RemoveAction((E_ActionScene, E_ActionList) key, System.Action action)
    {
        if (!D_Action.ContainsKey(key))
        {
            return;
        }
        D_Action[key] -= action;
        D_Action_Static[key] -= action;
    }
}

interface IAction
{
    Dictionary<(E_ActionScene, E_ActionList), System.Action> D_Action { get; set; }
    List<E_ActionScene> list { get; set; }
    void RemoveAction((E_ActionScene, E_ActionList) key, System.Action action);
    void AddAction((E_ActionScene, E_ActionList) key, System.Action action, bool isstatic = false);
    void SetAction((E_ActionScene, E_ActionList) key);
}

public enum E_ActionList
{
    Start,
    Play,
    End,
    Update,
    Boss,
}