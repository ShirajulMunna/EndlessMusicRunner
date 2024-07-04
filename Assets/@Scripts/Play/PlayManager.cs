using System.Collections.Generic;
using UnityEngine;

public class PlayManager : Singleton<PlayManager>
{
    Dictionary<E_Play, System.Action> D_Action = new Dictionary<E_Play, System.Action>();

    System.Action Ac_Play;

    private void Update()
    {
        SetAction(E_Play.Update);
    }

    public void SetAction(E_Play e_Play)
    {
        if (!D_Action.ContainsKey(e_Play))
        {
            return;
        }
        D_Action[e_Play]?.Invoke();
    }

    public void AddAction(E_Play e_Play, System.Action action)
    {
        RemoveAction(e_Play, action);
        if (!D_Action.ContainsKey(e_Play))
        {
            D_Action[e_Play] = null;
        }
        D_Action[e_Play] += action;
    }

    public void RemoveAction(E_Play e_Play, System.Action action)
    {
        if (!D_Action.ContainsKey(e_Play))
        {
            return;
        }
        D_Action[e_Play] -= action;
    }

}

public enum E_Play
{
    Play,
    End,
    Update
}