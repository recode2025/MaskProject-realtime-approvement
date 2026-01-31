using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 液体填充动画控制器 - MG风格快速填充效果
/// 完全使用脚本实现，无需额外资源
/// </summary>
public class LiquidController : MonoBehaviour
{
    [Header("组件引用")]
    public RectTransform liquidContainer;  // 液体容器（自动创建）
    public CanvasGroup menuPanel;  // 最终显示的菜单面板

    [Header("液体颜色")]
    public Color backWaveColor = new Color(1f, 0.5f, 0f, 1f);  // 后层深橙色
    public Color frontWaveColor = new Color(1f, 0.65f, 0.2f, 1f); // 前层浅橙色

    [Header("动画设置")]
    public float duration = 0.8f;  // 填充总时长（MG风格快速）
    public AnimationCurve fillCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 填充曲线
    public float frontWaveDelay = 0.05f; // 前层波浪延迟
    public float menuFadeStartTime = 0.7f; // 菜单淡入开始时间（相对于总时长的比例）
    public float menuFadeDuration = 0.3f; // 菜单淡入持续时间
    
    [Header("波浪设置")]
    public float waveHeight = 20f; // 波浪高度
    public float waveSpeed = 3f; // 波浪速度
    public int waveSegments = 50; // 波浪分段数（越多越平滑）
    
    [Header("调试")]
    public bool showDebugLog = false;
    
    private WaveLiquidUI backWave;
    private WaveLiquidUI frontWave;
    
    private Coroutine animationCoroutine;
    
    void Awake()
    {
        // 自动创建液体组件
        CreateLiquidComponents();
    }
    
    /// <summary>
    /// 创建液体UI组件
    /// </summary>
    private void CreateLiquidComponents()
    {
        if (liquidContainer == null)
        {
            // 创建液体容器
            GameObject containerObj = new GameObject("LiquidContainer");
            containerObj.transform.SetParent(transform, false);
            liquidContainer = containerObj.AddComponent<RectTransform>();
            liquidContainer.anchorMin = Vector2.zero;
            liquidContainer.anchorMax = Vector2.one;
            liquidContainer.sizeDelta = Vector2.zero;
            liquidContainer.anchoredPosition = Vector2.zero;
            
            // 设置在菜单面板之前
            if (menuPanel != null)
            {
                liquidContainer.SetSiblingIndex(menuPanel.transform.GetSiblingIndex());
            }
        }
        
        // 创建后层波浪
        if (backWave == null)
        {
            GameObject backObj = new GameObject("BackWave");
            backObj.transform.SetParent(liquidContainer, false);
            backWave = backObj.AddComponent<WaveLiquidUI>();
            backWave.liquidColor = backWaveColor;
            backWave.waveHeight = waveHeight;
            backWave.waveSpeed = waveSpeed;
            backWave.waveSegments = waveSegments;
            backWave.fillAmount = 0f;
        }
        
        // 创建前层波浪
        if (frontWave == null)
        {
            GameObject frontObj = new GameObject("FrontWave");
            frontObj.transform.SetParent(liquidContainer, false);
            frontWave = frontObj.AddComponent<WaveLiquidUI>();
            frontWave.liquidColor = frontWaveColor;
            frontWave.waveHeight = waveHeight * 0.8f; // 前层波浪稍小
            frontWave.waveSpeed = waveSpeed * 1.2f; // 前层波浪稍快
            frontWave.waveSegments = waveSegments;
            frontWave.waveOffset = 0.5f; // 相位偏移
            frontWave.fillAmount = 0f;
        }
    }
    
    /// <summary>
    /// 播放打开动画
    /// </summary>
    public void PlayOpenAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        
        // 确保液体容器激活
        if (liquidContainer != null)
        {
            liquidContainer.gameObject.SetActive(true);
        }
        
        animationCoroutine = StartCoroutine(OpenAnimationCoroutine());
    }
    
    /// <summary>
    /// 打开动画协程 - MG风格快速填充
    /// </summary>
    private IEnumerator OpenAnimationCoroutine()
    {
        // 重置状态
        if (backWave != null) backWave.fillAmount = 0;
        if (frontWave != null) frontWave.fillAmount = 0;
        if (menuPanel != null) 
        {
            menuPanel.alpha = 0;
            menuPanel.gameObject.SetActive(true);
        }
        
        if (showDebugLog)
        {
            Debug.Log("[LiquidController] 开始播放打开动画");
        }
        
        float elapsedTime = 0f;
        float frontWaveStartTime = frontWaveDelay;
        float menuFadeStart = duration * menuFadeStartTime;
        
        while (elapsedTime < duration + menuFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // 更新后层波浪（立即开始）
            if (backWave != null)
            {
                float backProgress = Mathf.Clamp01(elapsedTime / duration);
                backWave.fillAmount = fillCurve.Evaluate(backProgress);
            }
            
            // 更新前层波浪（稍有延迟）
            if (frontWave != null && elapsedTime >= frontWaveStartTime)
            {
                float frontProgress = Mathf.Clamp01((elapsedTime - frontWaveStartTime) / duration);
                frontWave.fillAmount = fillCurve.Evaluate(frontProgress);
            }
            
            // 更新菜单淡入
            if (menuPanel != null && elapsedTime >= menuFadeStart)
            {
                float menuProgress = Mathf.Clamp01((elapsedTime - menuFadeStart) / menuFadeDuration);
                menuPanel.alpha = menuProgress;
            }
            
            yield return null;
        }
        
        // 确保最终状态正确
        if (backWave != null) backWave.fillAmount = 1f;
        if (frontWave != null) frontWave.fillAmount = 1f;
        if (menuPanel != null) menuPanel.alpha = 1f;
        
        if (showDebugLog)
        {
            Debug.Log("[LiquidController] 打开动画完成");
        }
        
        animationCoroutine = null;
    }
    
    /// <summary>
    /// 播放关闭动画
    /// </summary>
    public void PlayCloseAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        
        animationCoroutine = StartCoroutine(CloseAnimationCoroutine());
    }
    
    /// <summary>
    /// 关闭动画协程
    /// </summary>
    private IEnumerator CloseAnimationCoroutine()
    {
        if (showDebugLog)
        {
            Debug.Log("[LiquidController] 开始播放关闭动画");
        }
        
        float elapsedTime = 0f;
        float closeDuration = duration * 0.5f; // 关闭动画更快
        
        // 先淡出菜单
        if (menuPanel != null)
        {
            float menuFadeTime = 0f;
            while (menuFadeTime < menuFadeDuration)
            {
                menuFadeTime += Time.deltaTime;
                menuPanel.alpha = 1f - Mathf.Clamp01(menuFadeTime / menuFadeDuration);
                yield return null;
            }
            menuPanel.alpha = 0f;
        }
        
        // 然后液体下降
        while (elapsedTime < closeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = 1f - Mathf.Clamp01(elapsedTime / closeDuration);
            
            if (backWave != null) backWave.fillAmount = progress;
            if (frontWave != null) frontWave.fillAmount = progress;
            
            yield return null;
        }
        
        // 确保最终状态正确
        if (backWave != null) backWave.fillAmount = 0f;
        if (frontWave != null) frontWave.fillAmount = 0f;
        
        if (showDebugLog)
        {
            Debug.Log("[LiquidController] 关闭动画完成");
        }
        
        animationCoroutine = null;
    }
    
    /// <summary>
    /// 立即设置为打开状态
    /// </summary>
    public void SetOpen()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        
        if (backWave != null) backWave.fillAmount = 1f;
        if (frontWave != null) frontWave.fillAmount = 1f;
        if (menuPanel != null) menuPanel.alpha = 1f;
    }
    
    /// <summary>
    /// 立即设置为关闭状态
    /// </summary>
    public void SetClosed()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        
        if (backWave != null) backWave.fillAmount = 0f;
        if (frontWave != null) frontWave.fillAmount = 0f;
        if (menuPanel != null) menuPanel.alpha = 0f;
    }
    
    /// <summary>
    /// 停止当前动画
    /// </summary>
    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }
    
    /// <summary>
    /// 检查是否正在播放动画
    /// </summary>
    public bool IsPlaying()
    {
        return animationCoroutine != null;
    }
    
    void OnValidate()
    {
        // 确保参数在合理范围内
        duration = Mathf.Max(0.1f, duration);
        menuFadeStartTime = Mathf.Clamp01(menuFadeStartTime);
        menuFadeDuration = Mathf.Max(0.1f, menuFadeDuration);
    }
}
