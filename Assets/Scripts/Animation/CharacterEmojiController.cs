using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 角色表情控制器
/// 点击角色显示随机表情，带淡入淡出动画
/// </summary>
public class CharacterEmojiController : MonoBehaviour, IPointerClickHandler
{
    [Header("角色设置")]
    [Tooltip("角色立绘Image")]
    public Image characterImage;
    
    [Header("表情设置")]
    [Tooltip("表情图片列表")]
    public Sprite[] emojiSprites;
    
    [Tooltip("表情显示的Image对象")]
    public Image emojiImage;
    
    [Header("动画设置")]
    [Tooltip("淡入时长（秒）")]
    public float fadeInDuration = 0.5f;
    
    [Tooltip("持续显示时长（秒）")]
    public float displayDuration = 2.0f;
    
    [Tooltip("淡出时长（秒）")]
    public float fadeOutDuration = 0.5f;
    
    [Header("调试")]
    [Tooltip("显示调试日志")]
    public bool showDebugLog = false;
    
    // 当前动画协程
    private Coroutine currentAnimation;
    
    // 总动画时长
    private float TotalDuration => fadeInDuration + displayDuration + fadeOutDuration;
    
    void Start()
    {
        // 检查角色立绘Image
        if (characterImage != null)
        {
            if (!characterImage.raycastTarget)
            {
                Debug.LogWarning($"[CharacterEmojiController] 角色立绘 {characterImage.name} 的 Raycast Target 未勾选！点击将无法触发。");
            }
            
            // 为角色立绘添加点击事件监听
            EventTrigger trigger = characterImage.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = characterImage.gameObject.AddComponent<EventTrigger>();
            }
            
            // 添加点击事件
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { OnCharacterClick((PointerEventData)data); });
            trigger.triggers.Add(entry);
            
            if (showDebugLog)
            {
                Debug.Log($"[CharacterEmojiController] 已为角色立绘 {characterImage.name} 添加点击监听");
            }
        }
        else
        {
            Debug.LogError("[CharacterEmojiController] characterImage 未设置！");
        }
        
        // 初始隐藏表情
        if (emojiImage != null)
        {
            emojiImage.gameObject.SetActive(false);
            
            if (showDebugLog)
            {
                Debug.Log($"[CharacterEmojiController] 初始化完成，共 {(emojiSprites != null ? emojiSprites.Length : 0)} 个表情");
            }
        }
        else
        {
            Debug.LogError("[CharacterEmojiController] emojiImage 未设置！");
        }
        
        // 检查表情列表
        if (emojiSprites == null || emojiSprites.Length == 0)
        {
            Debug.LogWarning("[CharacterEmojiController] 表情列表为空！");
        }
    }
    
    /// <summary>
    /// 角色被点击
    /// </summary>
    private void OnCharacterClick(PointerEventData eventData)
    {
        if (showDebugLog)
        {
            Debug.Log($"[CharacterEmojiController] 检测到点击角色立绘");
        }
        
        if (emojiImage == null || emojiSprites == null || emojiSprites.Length == 0)
        {
            Debug.LogWarning("[CharacterEmojiController] 无法显示表情：配置不完整");
            return;
        }
        
        // 停止当前动画
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            
            if (showDebugLog)
            {
                Debug.Log("[CharacterEmojiController] 停止当前动画");
            }
        }
        
        // 开始新动画
        currentAnimation = StartCoroutine(PlayEmojiAnimation());
    }
    
    /// <summary>
    /// 点击事件（保留用于兼容，实际使用 OnCharacterClick）
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        // 这个方法保留但不使用，实际点击通过 EventTrigger 触发 OnCharacterClick
    }
    
    /// <summary>
    /// 播放表情动画
    /// </summary>
    private IEnumerator PlayEmojiAnimation()
    {
        // 随机选择一个表情
        int randomIndex = Random.Range(0, emojiSprites.Length);
        Sprite selectedEmoji = emojiSprites[randomIndex];
        
        if (showDebugLog)
        {
            Debug.Log($"[CharacterEmojiController] 显示表情 #{randomIndex}");
        }
        
        // 设置表情图片
        emojiImage.sprite = selectedEmoji;
        emojiImage.gameObject.SetActive(true);
        
        // 淡入
        yield return StartCoroutine(FadeIn());
        
        // 持续显示
        yield return new WaitForSeconds(displayDuration);
        
        // 淡出
        yield return StartCoroutine(FadeOut());
        
        // 隐藏表情
        emojiImage.gameObject.SetActive(false);
        
        if (showDebugLog)
        {
            Debug.Log("[CharacterEmojiController] 表情动画完成");
        }
        
        currentAnimation = null;
    }
    
    /// <summary>
    /// 淡入动画
    /// </summary>
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = emojiImage.color;
        
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            color.a = alpha;
            emojiImage.color = color;
            
            yield return null;
        }
        
        // 确保完全不透明
        color.a = 1f;
        emojiImage.color = color;
    }
    
    /// <summary>
    /// 淡出动画
    /// </summary>
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = emojiImage.color;
        
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            color.a = alpha;
            emojiImage.color = color;
            
            yield return null;
        }
        
        // 确保完全透明
        color.a = 0f;
        emojiImage.color = color;
    }
}
