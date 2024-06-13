using Spine.Unity;

public interface IEntity_Spin
{
    SkeletonAnimation skeletonAnimation { get; set; }

    void SetAni((string, bool) data);
}