
using System.Collections.Generic;

interface ISpawnState
{
    int StageInfo { get; set; }
    List<C_LevelDesign> GetLevelDesigns();
    int GetStageInfo();
}