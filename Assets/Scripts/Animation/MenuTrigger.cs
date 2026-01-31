using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 菜单触发器 - 用于触发液体填充菜单动画
/// 可以绑定到按钮或其他UI元素上
/// </summary>
public class MenuTrigger : MonoBehaviour
{
    [Header("引用")]
    public LiquidController liquidController;
    
    [Header("自动查找")]
    public bool autoFindController = true;
    
    void Start()
    {
        if (autoFindController && liquidController == null)
        {
            liquidController = FindObjectOfType<LiquidController>();
        }
        
        // 如果是按钮，自动绑定点击事件
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OpenMenu);
        }
    }
    
    /// <summary>
    /// 打开菜单
    /// </summary>
    public void OpenMenu()
    {
        if (liquidController != null)
        {
            liquidController.PlayOpenAnimation();
        }
        else
        {
            Debug.LogWarning("[MenuTrigger] LiquidController 未设置！");
        }
    }
    
    /// <summary>
    /// 关闭菜单
    /// </summary>
    public void CloseMenu()
    {
        if (liquidController != null)
        {
            liquidController.PlayCloseAnimation();
        }
        else
        {
            Debug.LogWarning("[MenuTrigger] LiquidController 未设置！");
        }
    }
    
    /// <summary>
    /// 切换菜单状态
    /// </summary>
    public void ToggleMenu()
    {
        if (liquidController != null)
        {
            if (liquidController.IsPlaying())
            {
                return; // 动画播放中不响应
            }
            
            // 根据当前状态切换
            if (liquidController.menuPanel != null && liquidController.menuPanel.alpha > 0.5f)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }
}
