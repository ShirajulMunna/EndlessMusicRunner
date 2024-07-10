using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStage : Singleton<SpawnStage>, ISpawnState
{
    public int StageInfo { get; set; } = UI_Lobby.BitIdx;

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

