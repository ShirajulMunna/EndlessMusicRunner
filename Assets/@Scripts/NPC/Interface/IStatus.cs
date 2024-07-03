
public interface IStatus
{
    int MaxHp { get; set; }
    int CurrentHp { get; set; }
    float Speed { get; set; }
    int Damage { get; set; }
    void SetUp(int hp, float speed, int damage);

    void SetHp(int hp);
    int GetDamage();

    bool CheckDie();
}