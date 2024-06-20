using Unity.VisualScripting;

public enum E_AniType
{
    Running,
    Fly,
    Kick,
    Tail_Attack,
    Fire_Attack,
    Hold_Attack,
    Hit,
    Fly_Attack,
    Die,
    Hold_Fly_Attack,
    Clear, // 클리어 애니메이션추가
}

public enum E_AttackState
{
    None,
    Attack,
    Hold,
    Attack_Re,//다시 공격 가능 상태
    Twin_Attack,

}

//enum 순서를 Middle , UP으로 변경 
public enum E_MovePoint
{
    None = -1,
    Down,
    Middle,
    Up,
}

public enum E_PlayerSkill
{
    Fever,
    Heal,
    Shield,
    ScoreBooster,
    Running,
    Fly,
}
