using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour, IMonsterState
{
    public E_MonstersState e_MonstersState { get; set; }
    public Dictionary<E_MonstersState, System.Action> D_State_Start { get; set; } = new Dictionary<E_MonstersState, System.Action>();
    public Dictionary<E_MonstersState, System.Action> D_State_Update { get; set; } = new Dictionary<E_MonstersState, System.Action>();

    private void Update()
    {
        UpdateState();
    }

    public void UpdateState()
    {
        foreach (var item in D_State_Update)
        {
            if (item.Key != e_MonstersState)
            {
                return;
            }
            item.Value.Invoke();
        }
    }

    public void SetState(E_MonstersState state)
    {
        e_MonstersState = state;

        D_State_Start.TryGetValue(state, out var data);
        data?.Invoke();
    }

    public void AddStateAction_Start(E_MonstersState key, System.Action action)
    {
        var check = D_State_Start.ContainsKey(key);

        if (!check)
        {
            D_State_Start[key] = null;
        }

        D_State_Start[key] += action;
    }
    public void AddStateAction_Update(E_MonstersState key, System.Action action)
    {
        var check = D_State_Start.ContainsKey(key);

        if (!check)
        {
            D_State_Start[key] = null;
        }

        D_State_Start[key] += action;
    }
}