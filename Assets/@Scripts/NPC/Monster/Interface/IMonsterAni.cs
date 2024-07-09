public interface IMonsterAni
{
    int HitCount { get; set; }

    void SetAni(E_AniKind_Monster state, bool loop);
    void SetAni(string aniName, bool loop);
}