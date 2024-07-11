using System.Collections.Generic;
using Spine.Unity;

public interface IAni
{
    SkeletonAnimation sk { get; set; }
    void SetPlayerSkin(string name);
    void SetAni(string str, bool loop, string idle, bool dir = false);
}