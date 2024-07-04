
interface ISpawnTimePoint
{
    SpawnDelay spawnDelay { get; set; }
    ToolData toolData { get; set; }
    void SetUp(string name);
    bool CheckTime(double times);
    void RemoveTimes();
    bool CheckEndTiems();
}