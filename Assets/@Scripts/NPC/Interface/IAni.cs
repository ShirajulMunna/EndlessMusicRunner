using System.Collections.Generic;
using Spine.Unity;

public interface IAni
{
    SkeletonAnimation sk { get; set; }
    public List<string> skin_Names { get; set; }
    void SetPlayerSkin();
    void SetAni(string str, bool loop, string idle, bool dir = false);
}