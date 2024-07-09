public interface IMonsterAttack
{
    //공격 처리
    System.Action Ac_Attack { get; set; }
    bool isCheckAttack { get; set; }
    MonsterState monsterState { get; set; }

    //공격 가능 업데이트
    void UpdateAttack();

    //공격 가능 체크
    bool CheckAttack();

    //공격 처리 추가
    void AddAttack(System.Action action);
    //공격 가능 여부 셋팅
    void SetisAttack(bool state);
}