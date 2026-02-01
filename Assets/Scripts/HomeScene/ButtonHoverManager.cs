using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 按钮悬停管理器
/// 统一管理多个按钮和对应文本的悬停显示
/// </summary>
public class ButtonHoverManager : MonoBehaviour
{
    [System.Serializable]
    public class ButtonTextPair
    {
        [Tooltip("按钮对象")]
        public Button button;
        
        [Tooltip("对应的文本")]
        public Text hoverText;
    }
    
    [Header("按钮文本配对")]
    [Tooltip("按钮和文本的配对列表")]
    public ButtonTextPair[] buttonTextPairs = new ButtonTextPair[4];
    
    [Header("调试")]
    [Tooltip("显示调试日志")]
    public bool showDebugLog = false;
    
    void Start()
    {
        // 为每个按钮添加ButtonHoverText组件
        foreach (var pair in buttonTextPairs)
        {
            if (pair.button != null && pair.hoverText != null)
            {
                // 初始隐藏文本
                pair.hoverText.gameObject.SetActive(false);
                
                // 添加或获取ButtonHoverText组件
                ButtonHoverText hoverComponent = pair.button.gameObject.GetComponent<ButtonHoverText>();
                if (hoverComponent == null)
                {
                    hoverComponent = pair.button.gameObject.AddComponent<ButtonHoverText>();
                }
                
                // 设置关联的文本
                hoverComponent.hoverText = pair.hoverText;
                hoverComponent.showDebugLog = showDebugLog;
                
                if (showDebugLog)
                {
                    Debug.Log($"[ButtonHoverManager] 配置按钮: {pair.button.name} -> 文本: {pair.hoverText.name}");
                }
            }
            else
            {
                if (pair.button == null)
                {
                    Debug.LogWarning($"[ButtonHoverManager] 有按钮未设置！");
                }
                if (pair.hoverText == null)
                {
                    Debug.LogWarning($"[ButtonHoverManager] 有文本未设置！");
                }
            }
        }
        
        if (showDebugLog)
        {
            Debug.Log($"[ButtonHoverManager] 初始化完成，共配置 {buttonTextPairs.Length} 个按钮");
        }
    }
}
