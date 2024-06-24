using System.Collections.Generic;
using UnityEngine;

public class SpawnStage : MonoBehaviour, ISpawnState
{
    public static SpawnStage instance;
    public int StageInfo { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public List<C_LevelDesign> GetLevelDesigns()
    {
        var level = GameData.Data.LevelDesigin[StageInfo];
        return level;
    }

    public int GetStageInfo()
    {
        return StageInfo;
    }

}


interface ISpawnState
{
    int StageInfo { get; set; }
    List<C_LevelDesign> GetLevelDesigns();
    int GetStageInfo();
}