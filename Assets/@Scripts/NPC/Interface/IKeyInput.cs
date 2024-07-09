using System.Collections.Generic;
using UnityEngine;

interface IKeyInput
{
    Dictionary<KeyCode, System.Action> D_KeyInput_Down { get; set; }
    Dictionary<KeyCode, System.Action> D_KeyInput_Up { get; set; }
    Dictionary<(KeyCode, KeyCode), System.Action> D_TwinKeyInput_Down { get; set; }
    Dictionary<(KeyCode, KeyCode), System.Action> D_TwinKeyInput_Up { get; set; }
    //추가
    void AddKeyPoint_Down(KeyCode code, System.Action action);
    //제거
    void RemoveKeyPoint_Down(KeyCode code, System.Action action);

    void AddKeyPoint_Up(KeyCode code, System.Action action);
    //제거
    void RemoveKeyPoint_Up(KeyCode code, System.Action action);

    //트윈 키 입력
    void AddTwinKeyPoint_Down(KeyCode code1, KeyCode code2, System.Action action);
    //트윈 키 제거
    void RemoveTwinKeyPoint_Down(KeyCode code1, KeyCode code2, System.Action action);

    //트윈 키업 입력
    void AddTwinKeyPoint_Up(KeyCode code1, KeyCode code2, System.Action action);
    //트윈 키업 제거
    void RemoveTwinKeyPoint_Up(KeyCode code1, KeyCode code2, System.Action action);

    void UpdateKeyDown();
    void UpdateKeyUp();
    void UpdateKeyTwin_Down();
    void UpdateKeyTiwn_Up();
}