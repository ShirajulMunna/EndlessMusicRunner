public interface IEntity_Hp
{
    int MaxHp { get; set; }
    int CurHp { get; set; }

    void SetHp(int value);
    void SetAddHp(int value);
    void SetMinusHp(int value);
    bool CheckDie();
    void SetDie();
}

public class Entity_Hp : IEntity_Hp
{
    public int MaxHp { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int CurHp { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public bool CheckDie()
    {
        throw new System.NotImplementedException();
    }

    public void SetAddHp(int value)
    {
        throw new System.NotImplementedException();
    }

    public void SetDie()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetHp(int value)
    {
        throw new System.NotImplementedException();
    }

    public void SetMinusHp(int value)
    {
        throw new System.NotImplementedException();
    }
}