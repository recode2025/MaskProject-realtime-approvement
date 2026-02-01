using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 特殊模式视觉控制器
/// 监听 GameManager 的特殊模式状态，切换背景和角色
/// </summary>
public class SpecialModeVisualController : MonoBehaviour
{
    [Header("背景设置")]
    [Tooltip("普通背景 Image")]
    public Image normalBackground;
    
    [Tooltip("特殊背景 Image")]
    public Image specialBackground;
    
    [Header("角色设置")]
    [Tooltip("普通角色 GameObject")]
    public GameObject normalCharacter;
    
    [Tooltip("特殊角色 GameObject")]
    public GameObject specialCharacter;
    
    [Header("渐变设置")]
    [Tooltip("背景渐变时长（秒）")]
    public float transitionDuration = 1.0f;
    
    [Header("调试")]
    [Tooltip("显示调试日志")]
    public bool showDebugLog = false;
    
    // 当前是否在特殊模式
    private bool isInSpecialMode = false;
    
    // 渐变协程
    private Coroutine transitionCoroutine;
    
    void Start()
    {
        // 初始化状态
        InitializeVisuals();
        
        if (showDebugLog)
        {
            Debug.Log("[SpecialModeVisualController] 初始化完成");
        }
    }
    
    void OnEnable()
    {
        // 订阅特殊模式变化事件
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSpecialModeChanged += OnSpecialModeChanged;
            
            if (showDebugLog)
            {
                Debug.Log("[SpecialModeVisualController] 已订阅特殊模式事件");
            }
        }
        else
        {
            Debug.LogWarning("[SpecialModeVisualController] GameManager.Instance 为 null");
        }
    }
    
    void OnDisable()
    {
        // 取消订阅
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnSpecialModeChanged -= OnSpecialModeChanged;
            
            if (showDebugLog)
            {
                Debug.Log("[SpecialModeVisualController] 已取消订阅特殊模式事件");
            }
        }
        
        // 停止协程
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
            transitionCoroutine = null;
        }
    }
    
    /// <summary>
    /// 初始化视觉元素
    /// </summary>
    private void InitializeVisuals()
    {
        // 检查背景
        if (normalBackground == null || specialBackground == null)
        {
            Debug.LogError("[SpecialModeVisualController] 背景 Image 未设置！");
            return;
        }
        
        // 检查角色
        if (normalCharacter == null || specialCharacter == null)
        {
            Debug.LogError("[SpecialModeVisualController] 角色 GameObject 未设置！");
            return;
        }
        
        // 设置初始状态（普通模式）
        normalBackground.gameObject.SetActive(true);
        specialBackground.gameObject.SetActive(true);
        
        // 特殊背景初始完全透明
        Color specialColor = specialBackground.color;
        specialColor.a = 0f;
        specialBackground.color = specialColor;
        
        // 角色初始状态
        normalCharacter.SetActive(true);
        specialCharacter.SetActive(false);
        
        isInSpecialMode = false;
        
        if (showDebugLog)
        {
            Debug.Log("[SpecialModeVisualController] 初始化为普通模式");
        }
    }
    
    /// <summary>
    /// 特殊模式状态改变
    /// </summary>
    private void OnSpecialModeChanged(bool isSpecial)
    {
        if (showDebugLog)
        {
            Debug.Log($"[SpecialModeVisualController] 特殊模式变化: {isSpecial}");
        }
        
        // 如果状态没有变化，忽略
        if (isInSpecialMode == isSpecial)
        {
            return;
        }
        
        isInSpecialMode = isSpecial;
        
        // 停止当前渐变
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        
        // 开始新的渐变
        if (isSpecial)
        {
            // 进入特殊模式
            transitionCoroutine = StartCoroutine(TransitionToSpecialMode());
        }
        else
        {
            // 退出特殊模式
            transitionCoroutine = StartCoroutine(TransitionToNormalMode());
        }
    }
    
    /// <summary>
    /// 渐变到特殊模式
    /// </summary>
    private IEnumerator TransitionToSpecialMode()
    {
        if (showDebugLog)
        {
            Debug.Log("[SpecialModeVisualController] 开始渐变到特殊模式");
        }
        
        // 立即切换角色
        normalCharacter.SetActive(false);
        specialCharacter.SetActive(true);
        
        // 渐变背景：特殊背景从左到右显现（通过改变 fillAmount）
        // 使用 Image 的 Fill 功能
        if (specialBackground.type != Image.Type.Filled)
        {
            specialBackground.type = Image.Type.Filled;
            specialBackground.fillMethod = Image.FillMethod.Horizontal;
            specialBackground.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
        
        float elapsedTime = 0f;
        Color specialColor = specialBackground.color;
        specialColor.a = 1f;
        specialBackground.color = specialColor;
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            
            // 从左到右填充
            specialBackground.fillAmount = Mathf.Lerp(0f, 1f, t);
            
            yield return null;
        }
        
        // 确保完全显示
        specialBackground.fillAmount = 1f;
        
        if (showDebugLog)
        {
            Debug.Log("[SpecialModeVisualController] 特殊模式渐变完成");
        }
        
        transitionCoroutine = null;
    }
    
    /// <summary>
    /// 渐变回普通模式
    /// </summary>
    private IEnumerator TransitionToNormalMode()
    {
        if (showDebugLog)
        {
            Debug.Log("[SpecialModeVisualController] 开始渐变回普通模式");
        }
        
        // 立即切换角色
        specialCharacter.SetActive(false);
        normalCharacter.SetActive(true);
        
        // 渐变背景：特殊背景从左到右消失
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            
            // 从右到左清空（反向）
            specialBackground.fillAmount = Mathf.Lerp(1f, 0f, t);
            
            yield return null;
        }
        
        // 确保完全隐藏
        specialBackground.fillAmount = 0f;
        
        if (showDebugLog)
        {
            Debug.Log("[SpecialModeVisualController] 普通模式渐变完成");
        }
        
        transitionCoroutine = null;
    }
    
    /// <summary>
    /// 手动切换到特殊模式（用于测试）
    /// </summary>
    public void TestSwitchToSpecial()
    {
        OnSpecialModeChanged(true);
    }
    
    /// <summary>
    /// 手动切换到普通模式（用于测试）
    /// </summary>
    public void TestSwitchToNormal()
    {
        OnSpecialModeChanged(false);
    }
}
