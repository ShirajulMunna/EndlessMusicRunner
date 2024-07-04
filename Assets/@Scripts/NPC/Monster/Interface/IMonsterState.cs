using System.Collections.Generic;

interface IMonsterState
{
    E_MonstersState e_MonstersState { get; set; }
    Dictionary<E_MonstersState, System.Action> D_State_Start { get; set; }
    Dictionary<E_MonstersState, System.Action> D_State_Update { get; set; }
    void SetState(E_MonstersState state);
    void UpdateState();
    void AddStateAction_Start(E_MonstersState key, System.Action action);
    void AddStateAction_Update(E_MonstersState key, System.Action action);
}