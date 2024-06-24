using System.Diagnostics;

public interface ISkillClass
{
    IComboChecker ComboChecker { get; set; }
    ICoolTimeChecker CoolTimeChecker { get; set; }
    IActiveChecker ActiveChecker { get; set; }
    void SetData();
}

public class SkillClass : ISkillClass
{
    IComboChecker _ComboChecker;
    ICoolTimeChecker _CoolTimeChecker;
    IActiveChecker _ActiveChecker;


    public IComboChecker ComboChecker
    {
        get
        {
            if (_ComboChecker == null)
            {
                _ComboChecker = new ComboChecker();
            }
            return _ComboChecker;
        }
        set
        {

        }
    }
    public ICoolTimeChecker CoolTimeChecker
    {
        get
        {
            if (_CoolTimeChecker == null)
            {
                _CoolTimeChecker = new CoolTimeChecker();
            }
            return _CoolTimeChecker;
        }
        set
        {

        }
    }
    public IActiveChecker ActiveChecker
    {
        get
        {
            if (_ActiveChecker == null)
            {
                _ActiveChecker = new ActiveCheckter();
            }
            return _ActiveChecker;
        }
        set
        {

        }
    }

    //클래스 생성
    public static ISkillClass CreateClass(ISkillClass data)
    {
        if (data == null)
        {
            data = new SkillClass();
            data.SetData();
        }
        return data;
    }
    public void SetData()
    {
        CoolTimeChecker.SetCoolTimeData();
    }
}