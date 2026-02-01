using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 按钮悬停显示文本
/// 鼠标悬停在按钮上时显示对应的文本，移出时隐藏
/// </summary>
public class ButtonHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("关联的文本")]
    [Tooltip("鼠标悬停时显示的Text组件")]
    public Text hoverText;
    
    [Header("调试")]
    [Tooltip("显示调试日志")]
    public bool showDebugLog = false;
    
    void Start()
    {
        if (hoverText != null)
        {
            // 初始隐藏文本
            hoverText.gameObject.SetActive(false);
            
            if (showDebugLog)
            {
                Debug.Log($"[ButtonHoverText] 初始化 {gameObject.name}，文本已隐藏");
            }
        }
        else
        {
            Debug.LogWarning($"[ButtonHoverText] {gameObject.name} 未设置 hoverText！");
        }
    }
    
    /// <summary>
    /// 鼠标进入按钮
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(true);
            
            if (showDebugLog)
            {
                Debug.Log($"[ButtonHoverText] 鼠标进入 {gameObject.name} - 显示文本");
            }
        }
    }
    
    /// <summary>
    /// 鼠标离开按钮
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverText != null)
        {
            hoverText.gameObject.SetActive(false);
            
            if (showDebugLog)
            {
                Debug.Log($"[ButtonHoverText] 鼠标离开 {gameObject.name} - 隐藏文本");
            }
        }
    }
}
