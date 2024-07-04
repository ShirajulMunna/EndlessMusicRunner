public interface IPatern
{
    bool isPlay { get; set; }
    Bosst bosst { get; set; }
    void PlayPattern();
    void EndPattern();
}