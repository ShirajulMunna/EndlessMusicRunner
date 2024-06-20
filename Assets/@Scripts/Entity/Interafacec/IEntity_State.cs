using Spine.Unity;

public interface IEntity_State
{
    E_Entity_State e_Entity_State { get; set; }
    public void SetState(E_Entity_State state);
    public void UpdateState();
    public E_Entity_State GetState();
    public void SetRunning();
    public void SetHit();
    public void SetFly();
    public void UpdateDie();
    public void UpdateClear();
    public void UpdateIdle();
}


public enum E_Entity_State
{
    None = -1,
    Hit,
    Running,
    Die,
    Clear,
    Fly
}