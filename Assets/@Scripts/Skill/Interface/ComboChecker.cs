public interface IComboChecker
{
    bool CheckComboCondition(int combo);
}

public class ComboChecker : IComboChecker
{
    public bool CheckComboCondition(int combo)
    {
        return ScoreManager._instance.GetCurrentombo() >= combo;
    }
}
