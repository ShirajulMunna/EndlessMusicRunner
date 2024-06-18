public enum E_AniType
{
    Running,
    Fly,
    Kick,
    Tail_Attack,
    Fire_Attack,
    Hold_Attack,
    Hit,
    Fly_Attack
}

public enum E_AttackState
{
    None,
    Attack,
    Hold,
    Attack_Re,//다시 공격 가능 상태

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