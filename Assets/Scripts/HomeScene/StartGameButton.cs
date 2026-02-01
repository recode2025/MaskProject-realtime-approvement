using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 开始游戏按钮
/// 点击后触发场景过渡
/// </summary>
[RequireComponent(typeof(Button))]
public class StartGameButton : MonoBehaviour
{
    [Header("目标场景")]
    [Tooltip("要跳转的场景名称")]
    public string targetSceneName = "GameScene";
    
    [Header("调试")]
    [Tooltip("显示调试日志")]
    public bool showDebugLog = false;
    
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        
        // 添加点击事件监听
        button.onClick.AddListener(OnStartGameClicked);
        
        if (showDebugLog)
        {
            Debug.Log($"[StartGameButton] 初始化完成，目标场景: {targetSceneName}");
        }
    }
    
    /// <summary>
    /// 开始游戏按钮点击事件
    /// </summary>
    private void OnStartGameClicked()
    {
        if (showDebugLog)
        {
            Debug.Log($"[StartGameButton] 点击开始游戏，准备切换到: {targetSceneName}");
        }
        
        // 检查SceneTransition是否存在
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.TransitionToScene(targetSceneName);
        }
        else
        {
            Debug.LogError("[StartGameButton] SceneTransition.Instance 不存在！请确保场景中有SceneTransition组件。");
        }
    }
    
    void OnDestroy()
    {
        // 移除监听，避免内存泄漏
        if (button != null)
        {
            button.onClick.RemoveListener(OnStartGameClicked);
        }
    }
}
