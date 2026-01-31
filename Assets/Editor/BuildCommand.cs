using UnityEditor;

public static class BuildCommand
{
    public static void SetConfig()
    {
        // 替换为你真实的包名
        string bundleId = "com.7XL.dokidoki"; 
        
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleId);
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, bundleId);
        
        // 也可以顺便设置版本号
        PlayerSettings.bundleVersion = "1.0.0";
        
        AssetDatabase.SaveAssets();
    }
}