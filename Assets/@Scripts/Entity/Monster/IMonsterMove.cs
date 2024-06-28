public interface IMonsterMove
{
    float Speed { get; set; }
    float DestoryX { get; set; }
    void SetMove(int dirx = -1);
}