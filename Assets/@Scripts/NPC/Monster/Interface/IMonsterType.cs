public interface IMonsterType
{
    E_MonsterType e_MonsterType { get; set; }
    //타입 가져오기
    E_MonsterType GetMonsterType();

    //타입 설정
    void SetMonsterType(E_MonsterType e_monsterTypes);
}