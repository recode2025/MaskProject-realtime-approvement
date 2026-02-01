using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 场景过渡自动设置工具
/// 自动配置推拉门的位置、大小和缩放
/// </summary>
public class SceneTransitionSetup : MonoBehaviour
{
    [Header("推拉门图片")]
    [Tooltip("左侧门图片")]
    public Image leftDoor;
    
    [Tooltip("右侧门图片")]
    public Image rightDoor;
    
    [Header("调试信息")]
    [Tooltip("显示调试日志")]
    public bool showDebugLog = true;
    
    private Canvas canvas;
    
    /// <summary>
    /// 自动设置门的位置和大小
    /// </summary>
    public void SetupDoors()
    {
        if (leftDoor == null || rightDoor == null)
        {
            Debug.LogError("[SceneTransitionSetup] 请先设置左右门图片！");
            return;
        }
        
        // 直接使用屏幕尺寸
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // 计算每扇门的宽度（屏幕宽度的一半）
        float doorWidth = screenWidth / 2f;
        
        if (showDebugLog)
        {
            Debug.Log($"[SceneTransitionSetup] 屏幕尺寸: {screenWidth}x{screenHeight}");
            Debug.Log($"[SceneTransitionSetup] 门尺寸: {doorWidth}x{screenHeight}");
        }
        
        // 设置左门（pivot在右边缘中点）
        SetupLeftDoor(leftDoor, doorWidth, screenHeight);
        
        // 设置右门（pivot在左边缘中点）
        SetupRightDoor(rightDoor, doorWidth, screenHeight);
        
        Debug.Log("[SceneTransitionSetup] 门设置完成！");
    }
    
    /// <summary>
    /// 设置左门（pivot在右边缘中点）
    /// </summary>
    private void SetupLeftDoor(Image door, float width, float height)
    {
        RectTransform rectTransform = door.rectTransform;
        
        // 设置锚点为屏幕中心
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        
        // 设置pivot在右边缘中点（1, 0.5）
        rectTransform.pivot = new Vector2(1f, 0.5f);
        
        // 设置大小
        rectTransform.sizeDelta = new Vector2(width, height);
        
        // 设置位置（初始在屏幕左侧外，关闭时右边缘对齐屏幕中心）
        // 起始位置：X = -width（完全在屏幕外）
        // 关闭位置：X = 0（右边缘对齐屏幕中心）
        rectTransform.anchoredPosition = new Vector2(-width, 0);
        
        // 重置旋转和缩放
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
        
        if (showDebugLog)
        {
            Debug.Log($"[SceneTransitionSetup] 左门 - Pivot: (1, 0.5), 位置: ({-width}, 0), 大小: ({width}, {height})");
        }
    }
    
    /// <summary>
    /// 设置右门（pivot在左边缘中点）
    /// </summary>
    private void SetupRightDoor(Image door, float width, float height)
    {
        RectTransform rectTransform = door.rectTransform;
        
        // 设置锚点为屏幕中心
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        
        // 设置pivot在左边缘中点（0, 0.5）
        rectTransform.pivot = new Vector2(0f, 0.5f);
        
        // 设置大小
        rectTransform.sizeDelta = new Vector2(width, height);
        
        // 设置位置（初始在屏幕右侧外，关闭时左边缘对齐屏幕中心）
        // 起始位置：X = width（完全在屏幕外）
        // 关闭位置：X = 0（左边缘对齐屏幕中心）
        rectTransform.anchoredPosition = new Vector2(width, 0);
        
        // 重置旋转和缩放
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
        
        if (showDebugLog)
        {
            Debug.Log($"[SceneTransitionSetup] 右门 - Pivot: (0, 0.5), 位置: ({width}, 0), 大小: ({width}, {height})");
        }
    }
}
