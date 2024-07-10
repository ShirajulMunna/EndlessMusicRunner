public interface IMonsterAni
{
    int HitCount { get; set; }

    void SetAni(E_AniKind_Monster state, bool loop, bool dir = false);
    void SetAni(string aniName, bool loop);
}