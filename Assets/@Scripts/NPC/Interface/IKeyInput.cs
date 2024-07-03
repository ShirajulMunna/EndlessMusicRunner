using System.Collections.Generic;
using UnityEngine;

interface IKeyInput
{
    Dictionary<KeyCode, System.Action> D_KeyInput_Down { get; set; }
    Dictionary<KeyCode, System.Action> D_KeyInput_Up { get; set; }
    Dictionary<(KeyCode, KeyCode), System.Action> D_TwinKeyInput { get; set; }
    //추가
    void AddKeyPoint_Down(KeyCode code, System.Action action);
    //제거
    void RemoveKeyPoint_Down(KeyCode code, System.Action action);

    void AddKeyPoint_Up(KeyCode code, System.Action action);
    //제거
    void RemoveKeyPoint_Up(KeyCode code, System.Action action);

    //트윈 키 입력
    void AddTwinKeyPoint(KeyCode code1, KeyCode code2, System.Action action);
    //트윈 키 제거
    void RemoveTwinKeyPoint(KeyCode code1, KeyCode code2, System.Action action);
}