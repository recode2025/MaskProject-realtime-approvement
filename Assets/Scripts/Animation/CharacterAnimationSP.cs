using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 角色帧动画控制器 - SP版本（无抬手动作）
/// 动画序列：1 1 1 2 3 4 4 5 1
/// 完整循环后回到待机状态
/// </summary>
[RequireComponent(typeof(Image))]
public class CharacterAnimationSP : MonoBehaviour
{
    [Header("动画帧设置")]
    [Tooltip("5张动画帧图片")]
    public Sprite[] animationFrames = new Sprite[5];
    
    [Header("播放设置")]
    [Tooltip("每帧持续时间（秒），默认1帧 = 1/60秒")]
    public float frameTime = 1f / 60f;
    
    [Header("调试信息")]
    public bool showDebugLog = false;
    
    // 组件引用
    private Image characterImage;
    
    // 动画状态
    private bool isPlaying = false;
    private Coroutine animationCoroutine;
    
    // 当前显示的帧（0-4，对应图片1-5）
    private int currentFrame = 0;
    
    // 动画序列：1 1 1 2 3 4 4 5 1（索引：0 0 0 1 2 3 3 4 0）
    private readonly int[] animationSequence = { 0, 0, 1, 1, 1,2, 2, 2, 3, 3, 3, 3, 4, 4,4, 0 };
    
    void Awake()
    {
        characterImage = GetComponent<Image>();
        
        if (characterImage == null)
        {
            Debug.LogError("[CharacterAnimationSP] 未找到 Image 组件！");
        }
    }
    
    void Start()
    {
        // 设置默认第一帧
        SetFrame(0);
        
        if (showDebugLog)
        {
            Debug.Log("[CharacterAnimationSP] 角色动画控制器（SP版）已初始化");
        }
    }
    
    void OnEnable()
    {
        // 订阅输入事件
        InputSystem.OnPlayerInput += OnPlayerInputReceived;
        
        if (showDebugLog)
        {
            Debug.Log("[CharacterAnimationSP] 已订阅输入事件");
        }
    }
    
    void OnDisable()
    {
        // 取消订阅输入事件
        InputSystem.OnPlayerInput -= OnPlayerInputReceived;
        
        // 停止协程
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        
        if (showDebugLog)
        {
            Debug.Log("[CharacterAnimationSP] 已取消订阅输入事件");
        }
    }
    
    /// <summary>
    /// 当接收到玩家输入时触发
    /// </summary>
    private void OnPlayerInputReceived()
    {
        if (showDebugLog)
        {
            Debug.Log($"[CharacterAnimationSP] 收到输入 - 当前帧:{currentFrame + 1}, 播放中:{isPlaying}");
        }
        
        // 如果正在播放动画，忽略输入
        if (isPlaying)
        {
            if (showDebugLog)
            {
                Debug.Log("[CharacterAnimationSP] 动画播放中，忽略输入");
            }
            return;
        }
        
        // 开始播放动画
        StartAnimation();
    }
    
    /// <summary>
    /// 开始播放动画
    /// </summary>
    private void StartAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        
        animationCoroutine = StartCoroutine(PlayAnimationSequence());
    }
    
    /// <summary>
    /// 播放完整的动画序列
    /// </summary>
    private IEnumerator PlayAnimationSequence()
    {
        isPlaying = true;
        
        if (showDebugLog)
        {
            Debug.Log("[CharacterAnimationSP] 开始播放动画序列");
        }
        
        // 播放动画序列：1 1 1 2 3 4 4 5 1
        for (int i = 0; i < animationSequence.Length; i++)
        {
            currentFrame = animationSequence[i];
            SetFrame(currentFrame);
            
            if (showDebugLog)
            {
                Debug.Log($"[CharacterAnimationSP] 播放 [{i}/{animationSequence.Length - 1}] 显示帧 {currentFrame + 1}");
            }
            
            yield return new WaitForSeconds(frameTime);
        }
        
        // 动画完成，回到待机状态
        currentFrame = 0;
        SetFrame(0);
        isPlaying = false;
        animationCoroutine = null;
        
        if (showDebugLog)
        {
            Debug.Log("[CharacterAnimationSP] 动画完成，回到待机");
        }
    }
    
    /// <summary>
    /// 设置显示的帧
    /// </summary>
    private void SetFrame(int frameIndex)
    {
        if (characterImage == null || animationFrames == null || frameIndex < 0 || frameIndex >= animationFrames.Length)
        {
            return;
        }
        
        if (animationFrames[frameIndex] != null)
        {
            characterImage.sprite = animationFrames[frameIndex];
        }
        else
        {
            Debug.LogWarning($"[CharacterAnimationSP] 帧 {frameIndex + 1} 的图片未设置！");
        }
    }
    
    /// <summary>
    /// 验证动画帧是否完整
    /// </summary>
    private bool ValidateFrames()
    {
        if (animationFrames == null || animationFrames.Length != 5)
        {
            Debug.LogError("[CharacterAnimationSP] 动画帧数组必须包含5张图片！");
            return false;
        }
        
        for (int i = 0; i < animationFrames.Length; i++)
        {
            if (animationFrames[i] == null)
            {
                Debug.LogError($"[CharacterAnimationSP] 动画帧 {i + 1} 未设置！");
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// 手动触发动画（用于测试）
    /// </summary>
    public void TriggerAnimation()
    {
        OnPlayerInputReceived();
    }
    
    /// <summary>
    /// 强制停止动画
    /// </summary>
    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        
        isPlaying = false;
        currentFrame = 0;
        SetFrame(0);
        
        if (showDebugLog)
        {
            Debug.Log("[CharacterAnimationSP] 动画已强制停止");
        }
    }
    
    /// <summary>
    /// 检查是否正在播放
    /// </summary>
    public bool IsPlaying()
    {
        return isPlaying;
    }
    
    /// <summary>
    /// 获取当前帧索引（0-4）
    /// </summary>
    public int GetCurrentFrameIndex()
    {
        return currentFrame;
    }
}
