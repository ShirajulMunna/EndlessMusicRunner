public interface IComboChecker
{
    bool CheckComboCondition(int combo);
}

public class ComboChecker : IComboChecker
{
    public bool CheckComboCondition(int combo)
    {
        return ScoreManager.instance.GetCurrentombo() >= combo;
    }
}
