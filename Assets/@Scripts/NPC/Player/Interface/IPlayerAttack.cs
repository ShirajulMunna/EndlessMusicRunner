using System.Collections.Generic;
using UnityEngine;

interface IPlayerAttack
{
    Dictionary<E_MoveData, Vector3> Tr_AttackVector { get; set; }
    NPC nPC { get; set; }
    bool isTwin { get; set; }
    bool isHold { get; set; }
    List<Vector3> BoxSize { get; set; }

    //공격
    void SetAttack(E_MoveData idx);

    //공격 체크
    (Collider2D[], ScoreManager.E_ScoreState) SetAttack_Area(E_MoveData idx);

    //홀드 셋팅
    void SetHold(bool state);
    //홀드 확인
    bool CheckHold();

    //트윈 셋팅
    void SetTwin(bool state);
    //트윈 확인
    bool CheckTwin();

    //특수 몬스터 확인
    bool CheckSpecialMonster(IMonster types);

    //이펙트 처리
    bool CheckAttackState(GameObject obj, E_MoveData idx, ScoreManager.E_ScoreState scorestate);
}