using UnityEngine;
using UnityEditor;

/// <summary>
/// 用于设置 Sorting Layers 的编辑器工具
/// 使用方法：在 Unity 菜单栏选择 Tools → Setup Sorting Layers
/// </summary>
public class SortingLayerSetup : EditorWindow
{
    [MenuItem("Tools/Setup Sorting Layers for Sushi Game")]
    static void SetupSortingLayers()
    {
        // 注意：Unity 不允许通过脚本直接创建 Sorting Layers
        // 需要手动在 Edit → Project Settings → Tags and Layers 中创建
        
        string message = @"请按照以下步骤手动设置 Sorting Layers：

1. 打开 Edit → Project Settings → Tags and Layers
2. 展开 Sorting Layers 部分
3. 按照以下顺序添加 Sorting Layers（从上到下）：
   - Default（默认已存在）
   - Background（背景）
   - Plate（碟子）
   - Sushi（寿司）
   - Conveyor（传送带）
   - UI（其他UI）

4. 设置完成后：
   - 碟子会自动使用 'Plate' 层（sortingOrder = 0）
   - 寿司会自动使用 'Sushi' 层（sortingOrder = 10）
   - 传送带应该设置为 'Conveyor' 层
   - 其他UI元素设置为 'UI' 层

5. 如果不想创建 Sorting Layers：
   - 代码会自动使用 sortingOrder 来控制渲染顺序
   - 碟子：sortingOrder = -100
   - 寿司：sortingOrder = 10
   - 传送带：建议设置 sortingOrder = -50

注意：Sorting Layer 的顺序决定了渲染顺序，
列表中越靠下的层会渲染在越上面。";

        EditorUtility.DisplayDialog("设置 Sorting Layers", message, "我知道了");
    }

    [MenuItem("Tools/Check Sushi Sprites")]
    static void CheckSushiSprites()
    {
        // 检查寿司贴图的设置
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { "Assets/Resources/Images" });
        
        string report = "寿司贴图检查报告：\n\n";
        int issueCount = 0;
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("sushi") || path.Contains("suhi"))
            {
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer != null)
                {
                    bool hasIssue = false;
                    string issues = "";
                    
                    if (importer.textureType != TextureImporterType.Sprite)
                    {
                        issues += "  ❌ Texture Type 不是 Sprite\n";
                        hasIssue = true;
                    }
                    
                    if (importer.spriteImportMode != SpriteImportMode.Single)
                    {
                        issues += "  ⚠️ Sprite Mode 不是 Single\n";
                    }
                    
                    if (hasIssue)
                    {
                        report += $"文件: {path}\n{issues}\n";
                        issueCount++;
                    }
                    else
                    {
                        report += $"✅ {path}\n";
                    }
                }
            }
        }
        
        if (issueCount > 0)
        {
            report += $"\n发现 {issueCount} 个问题，建议修复。";
        }
        else
        {
            report += "\n所有寿司贴图设置正确！";
        }
        
        Debug.Log(report);
        EditorUtility.DisplayDialog("寿司贴图检查", report, "确定");
    }

    [MenuItem("Tools/Fix Sushi Sprite Settings")]
    static void FixSushiSpriteSettings()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Resources/Images" });
        
        int fixedCount = 0;
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("sushi") || path.Contains("suhi") || path.Contains("disk"))
            {
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer != null && importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.spritePixelsPerUnit = 100;
                    importer.filterMode = FilterMode.Bilinear;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                    fixedCount++;
                    
                    Debug.Log($"已修复: {path}");
                }
            }
        }
        
        if (fixedCount > 0)
        {
            EditorUtility.DisplayDialog("修复完成", $"已修复 {fixedCount} 个贴图的设置。", "确定");
        }
        else
        {
            EditorUtility.DisplayDialog("无需修复", "所有贴图设置都正确。", "确定");
        }
    }
}
