public static class BuildManager
{
    public static EPlatformType targetPlatform = EPlatformType.Android;

    public static bool IsPhone()
    {
        return targetPlatform.In(new EPlatformType[] { EPlatformType.IOS, EPlatformType.Android });
    }

    public static bool IsIOS()
    {
        return targetPlatform == EPlatformType.IOS;
    }

    public static bool IsAndroid()
    {
        return targetPlatform == EPlatformType.Android;
    }
}