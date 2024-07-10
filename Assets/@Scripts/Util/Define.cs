using System;
using System.Globalization;
using Spine;
using Spine.Unity;
using UnityEngine;

public static class Define
{
    //애니메이션 실행
    public static void SetAni(this SkeletonAnimation skeletonAnimation, string anistr, bool loop, string idle)
    {
        skeletonAnimation.AnimationState.ClearTracks(); // 모든 애니메이션 트랙을 제거
        SkeletonDataAsset skeletonDataAsset = skeletonAnimation.SkeletonDataAsset;

        SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(true);
        var animations = skeletonData.Animations.Items;
        float delay = 0f;
        // animationString에 해당하는 애니메이션의 Duration을 찾아 delay로 설정
        foreach (var anim in animations)
        {
            if (anim.Name == anistr)
            {
                delay = anim.Duration;
                break;
            }
        }
        skeletonAnimation.skeleton.SetToSetupPose();
        skeletonAnimation.AnimationState.SetAnimation(0, anistr, loop);

        if (loop == false)
        {
            skeletonAnimation.AnimationState.AddAnimation(0, idle, true, delay); // 찾은 딜레이 값을 AddAnimation에 적용
        }
    }

    public static void SetAni_Dir(this SkeletonAnimation skeletonAnimation, string anistr)
    {
        skeletonAnimation.AnimationState.ClearTracks(); // 모든 애니메이션 트랙을 제거
        SkeletonDataAsset skeletonDataAsset = skeletonAnimation.SkeletonDataAsset;

        SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(true);
        var animations = skeletonData.Animations.Items;
        float delay = 0f;
        // animationString에 해당하는 애니메이션의 Duration을 찾아 delay로 설정
        foreach (var anim in animations)
        {
            if (anim.Name == anistr)
            {
                delay = anim.Duration;
                break;
            }
        }
        skeletonAnimation.skeleton.SetToSetupPose();
        skeletonAnimation.AnimationState.SetAnimation(0, anistr, false);
    }
}


public static class Define_String
{
    public static TimeSpan GetTotalTime(this string dateTime_str)
    {
        var date = dateTime_str.GetDateTime();
        var total = DateTime.UtcNow - date;
        return total;
    }

    public static DateTime GetDateTime(this string time)
    {
        if (string.IsNullOrEmpty(time))
        {
            return default;
        }

        DateTime.TryParse(time, out var result);

        if (result == default)
        {
            if (DateTime.TryParseExact(time, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
        }
        return result;
    }
}
