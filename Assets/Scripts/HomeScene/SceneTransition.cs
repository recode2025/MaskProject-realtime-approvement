using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景过渡管理器
/// 实现推拉门效果的场景切换
/// </summary>
public class SceneTransition : MonoBehaviour
{
    // 单例模式
    public static SceneTransition Instance { get; private set; }
    
    [Header("推拉门图片")]
    [Tooltip("左侧门图片")]
    public Image leftDoor;
    
    [Tooltip("右侧门图片")]
    public Image rightDoor;
    
    [Header("过渡设置")]
    [Tooltip("门关闭动画时长（秒）")]
    public float closeDuration = 1.0f;
    
    [Tooltip("门打开动画时长（秒）")]
    public float openDuration = 1.0f;
    
    [Header("音效")]
    [Tooltip("场景切换音效")]
    public AudioClip transitionSound;
    
    [Tooltip("音效音量")]
    [Range(0f, 1f)]
    public float soundVolume = 1.0f;
    
    [Header("调试")]
    [Tooltip("显示调试日志")]
    public bool showDebugLog = false;
    
    // 音频源组件
    private AudioSource audioSource;
    
    // 门的初始位置
    private Vector2 leftDoorStartPos;
    private Vector2 rightDoorStartPos;
    
    // 门的目标位置（关闭时）
    private Vector2 leftDoorClosedPos;
    private Vector2 rightDoorClosedPos;
    
    // Canvas
    private Canvas canvas;
    
    void Awake()
    {
        // 单例模式 - 不使用DontDestroyOnLoad，让它跟随场景
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // 添加音频源组件
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        
        // 获取Canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[SceneTransition] 找不到父级Canvas！");
        }
    }
    
    void Start()
    {
        if (leftDoor != null && rightDoor != null)
        {
            // 初始化门的大小和位置
            InitializeDoors();
            
            // 初始时门在两侧外面（隐藏）
            leftDoor.rectTransform.anchoredPosition = leftDoorStartPos;
            rightDoor.rectTransform.anchoredPosition = rightDoorStartPos;
            
            if (showDebugLog)
            {
                Debug.Log("[SceneTransition] 初始化完成");
            }
        }
        else
        {
            Debug.LogError("[SceneTransition] 左右门图片未设置！");
        }
    }
    
    void OnEnable()
    {
        // 订阅场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        // 取消订阅
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    /// <summary>
    /// 场景加载完成回调
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (showDebugLog)
        {
            Debug.Log($"[SceneTransition] 场景加载完成: {scene.name}");
        }
        
        // 重新初始化门（适配新场景的分辨率）
        if (leftDoor != null && rightDoor != null)
        {
            InitializeDoors();
        }
    }
    
    /// <summary>
    /// 初始化门的大小和位置
    /// </summary>
    private void InitializeDoors()
    {
        // 获取屏幕尺寸
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        float canvasWidth;
        float canvasHeight;
        
        // 如果有Canvas，使用Canvas尺寸；否则使用屏幕尺寸
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            canvasWidth = canvasRect.rect.width;
            canvasHeight = canvasRect.rect.height;
        }
        else
        {
            canvasWidth = screenWidth;
            canvasHeight = screenHeight;
            
            if (showDebugLog)
            {
                Debug.LogWarning("[SceneTransition] Canvas为null，使用屏幕尺寸");
            }
        }
        
        // 计算每扇门的宽度（Canvas宽度的一半）
        float doorWidth = canvasWidth / 2f;
        
        // 设置门的尺寸
        leftDoor.rectTransform.sizeDelta = new Vector2(doorWidth, canvasHeight);
        rightDoor.rectTransform.sizeDelta = new Vector2(doorWidth, canvasHeight);
        
        // 设置锚点为屏幕中心
        leftDoor.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        leftDoor.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rightDoor.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rightDoor.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        
        // 左门：pivot在右边缘中点（1, 0.5）
        leftDoor.rectTransform.pivot = new Vector2(1f, 0.5f);
        
        // 右门：pivot在左边缘中点（0, 0.5）
        rightDoor.rectTransform.pivot = new Vector2(0f, 0.5f);
        
        // 计算位置
        // 左门：起始位置在左侧外（-doorWidth），关闭位置在屏幕中心（0）
        leftDoorStartPos = new Vector2(-doorWidth, 0);
        leftDoorClosedPos = new Vector2(0, 0);
        
        // 右门：起始位置在右侧外（doorWidth），关闭位置在屏幕中心（0）
        rightDoorStartPos = new Vector2(doorWidth, 0);
        rightDoorClosedPos = new Vector2(0, 0);
        
        if (showDebugLog)
        {
            Debug.Log($"[SceneTransition] 屏幕尺寸: {screenWidth}x{screenHeight}");
            Debug.Log($"[SceneTransition] Canvas尺寸: {canvasWidth}x{canvasHeight}");
            Debug.Log($"[SceneTransition] 门尺寸: {doorWidth}x{canvasHeight}");
            Debug.Log($"[SceneTransition] 左门 - Pivot: (1, 0.5), 起始: {leftDoorStartPos}, 关闭: {leftDoorClosedPos}");
            Debug.Log($"[SceneTransition] 右门 - Pivot: (0, 0.5), 起始: {rightDoorStartPos}, 关闭: {rightDoorClosedPos}");
        }
    }
    
    /// <summary>
    /// 切换场景（带过渡动画）
    /// </summary>
    /// <param name="sceneName">目标场景名称</param>
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(TransitionCoroutine(sceneName));
    }
    
    /// <summary>
    /// 场景过渡协程
    /// </summary>
    private IEnumerator TransitionCoroutine(string sceneName)
    {
        if (showDebugLog)
        {
            Debug.Log($"[SceneTransition] 开始切换到场景: {sceneName}");
        }
        
        // 播放音效
        if (transitionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(transitionSound, soundVolume);
            
            if (showDebugLog)
            {
                Debug.Log("[SceneTransition] 播放音效");
            }
        }
        
        // 关闭门（从两侧移动到中间）
        yield return StartCoroutine(CloseDoors());
        
        // 标记为DontDestroyOnLoad，确保过渡到下一个场景时不被销毁
        // 如果canvas为null，则标记当前GameObject
        if (canvas != null)
        {
            DontDestroyOnLoad(canvas.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            if (showDebugLog)
            {
                Debug.LogWarning("[SceneTransition] Canvas为null，标记当前GameObject为DontDestroyOnLoad");
            }
        }
        
        // 加载场景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        yield return asyncLoad;
        
        if (showDebugLog)
        {
            Debug.Log($"[SceneTransition] 场景加载完成: {sceneName}");
        }
        
        // 等待一帧，确保场景完全加载
        yield return null;
        
        // 打开门（从中间移动到两侧）
        yield return StartCoroutine(OpenDoors());
        
        // 销毁Canvas或当前GameObject
        if (canvas != null)
        {
            Destroy(canvas.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (showDebugLog)
        {
            Debug.Log("[SceneTransition] 过渡完成，对象已销毁");
        }
        
        // 清空单例引用
        Instance = null;
    }
    
    /// <summary>
    /// 关闭门动画
    /// </summary>
    private IEnumerator CloseDoors()
    {
        float elapsedTime = 0f;
        
        Vector2 leftStart = leftDoor.rectTransform.anchoredPosition;
        Vector2 rightStart = rightDoor.rectTransform.anchoredPosition;
        
        while (elapsedTime < closeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / closeDuration;
            
            // 使用平滑曲线
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            leftDoor.rectTransform.anchoredPosition = Vector2.Lerp(leftStart, leftDoorClosedPos, smoothT);
            rightDoor.rectTransform.anchoredPosition = Vector2.Lerp(rightStart, rightDoorClosedPos, smoothT);
            
            yield return null;
        }
        
        // 确保到达最终位置
        leftDoor.rectTransform.anchoredPosition = leftDoorClosedPos;
        rightDoor.rectTransform.anchoredPosition = rightDoorClosedPos;
        
        if (showDebugLog)
        {
            Debug.Log("[SceneTransition] 门已关闭");
        }
    }
    
    /// <summary>
    /// 打开门动画
    /// </summary>
    private IEnumerator OpenDoors()
    {
        float elapsedTime = 0f;
        
        Vector2 leftStart = leftDoor.rectTransform.anchoredPosition;
        Vector2 rightStart = rightDoor.rectTransform.anchoredPosition;
        
        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / openDuration;
            
            // 使用平滑曲线
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            leftDoor.rectTransform.anchoredPosition = Vector2.Lerp(leftStart, leftDoorStartPos, smoothT);
            rightDoor.rectTransform.anchoredPosition = Vector2.Lerp(rightStart, rightDoorStartPos, smoothT);
            
            yield return null;
        }
        
        // 确保到达最终位置
        leftDoor.rectTransform.anchoredPosition = leftDoorStartPos;
        rightDoor.rectTransform.anchoredPosition = rightDoorStartPos;
        
        if (showDebugLog)
        {
            Debug.Log("[SceneTransition] 门已打开");
        }
    }
}
