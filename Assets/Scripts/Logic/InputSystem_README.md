# 输入系统使用说明

## 🎮 快速开始

### 1. 在场景中设置 InputSystem

1. 在场景中创建一个空的 GameObject，命名为 "InputSystem"
2. 将 `InputSystem.cs` 脚本挂载到这个 GameObject 上
3. 在 Inspector 中设置：
   - **Current Input Mode**: 选择输入模式（Touch/Keyboard/Gamepad）
   - **Keyboard Key**: 设置键盘按键（默认是 Space）
   - **Show Debug Log**: 勾选以查看调试信息

### 2. 测试输入是否工作

1. 创建一个空的 GameObject，命名为 "InputListener"
2. 将 `InputListener.cs` 脚本挂载到这个 GameObject 上
3. 勾选 **Show Debug Log**
4. 运行游戏，按下空格键（或点击屏幕），查看 Console 是否有输出

### 3. 在你的脚本中订阅输入事件

```csharp
using UnityEngine;

public class YourScript : MonoBehaviour
{
    void OnEnable()
    {
        // 订阅输入事件
        InputSystem.OnPlayerInput += OnPlayerInputReceived;
    }
    
    void OnDisable()
    {
        // 取消订阅（重要！）
        InputSystem.OnPlayerInput -= OnPlayerInputReceived;
    }
    
    private void OnPlayerInputReceived()
    {
        Debug.Log("收到玩家输入！");
        // 在这里执行你的判定逻辑
    }
}
```

## 🔧 自定义按键的三种方法

### 方法1：在 Inspector 中直接修改（最简单）

1. 选中场景中的 InputSystem GameObject
2. 在 Inspector 中找到 **Keyboard Key** 字段
3. 点击下拉菜单，选择你想要的按键（如 A, D, Enter 等）
4. 运行游戏测试

### 方法2：通过代码动态修改

```csharp
// 修改为 A 键
InputSystem.Instance.SetKeyboardKey(KeyCode.A);

// 修改为回车键
InputSystem.Instance.SetKeyboardKey(KeyCode.Return);

// 修改为鼠标左键（不推荐，因为会和触摸模式冲突）
InputSystem.Instance.SetKeyboardKey(KeyCode.Mouse0);
```

### 方法3：使用 UI 设置面板（推荐给玩家使用）

1. 创建一个 Canvas（如果还没有）
2. 在 Canvas 下创建设置面板 UI：
   - Dropdown（下拉菜单）- 用于选择输入模式
   - Text - 显示当前按键
   - Button - "更改按键"按钮
   - Text - 状态提示文本

3. 创建一个空的 GameObject，挂载 `InputSettingsUI.cs`
4. 在 Inspector 中将 UI 元素拖拽到对应的字段
5. 玩家点击"更改按键"按钮后，按下任意键即可设置

## 🐛 故障排查

### 问题1：按空格键没反应

**可能原因和解决方法：**

1. **InputSystem 没有在场景中**
   - 检查 Hierarchy 中是否有 InputSystem GameObject
   - 确保 InputSystem.cs 脚本已挂载

2. **当前输入模式不是 Keyboard**
   - 在 Inspector 中检查 **Current Input Mode** 是否设置为 **Keyboard**
   - 或者运行时在 Console 输入：`InputSystem.Instance.SwitchInputMode(InputMode.Keyboard)`

3. **没有订阅输入事件**
   - 确保你的脚本中有 `InputSystem.OnPlayerInput += YourMethod;`
   - 检查是否在 `OnEnable()` 中订阅

4. **Debug Log 没开启**
   - 勾选 InputSystem 的 **Show Debug Log**
   - 勾选 InputListener 的 **Show Debug Log**
   - 查看 Console 是否有输出

5. **按键被其他 UI 拦截**
   - 检查是否有 InputField 或其他 UI 元素获得了焦点
   - 尝试点击游戏画面后再按空格键

### 问题2：触摸/点击模式不工作

1. 确保 **Current Input Mode** 设置为 **Touch**
2. 如果使用了 `TouchInputButton.cs`，确保：
   - 它挂载在一个全屏的 Image 上
   - Image 的 **Raycast Target** 已勾选
   - Canvas 的 **Graphic Raycaster** 组件存在

### 问题3：手柄模式不工作

1. 确保手柄已连接
2. 在 Unity 的 **Edit > Project Settings > Input Manager** 中检查轴配置
3. 尝试按手柄的 A 键（Xbox）或 X 键（PlayStation）

## 📝 常用按键代码

```csharp
KeyCode.Space          // 空格键
KeyCode.Return         // 回车键
KeyCode.A              // A 键
KeyCode.D              // D 键
KeyCode.W              // W 键
KeyCode.S              // S 键
KeyCode.LeftArrow      // 左方向键
KeyCode.RightArrow     // 右方向键
KeyCode.UpArrow        // 上方向键
KeyCode.DownArrow      // 下方向键
KeyCode.LeftShift      // 左 Shift 键
KeyCode.LeftControl    // 左 Ctrl 键
KeyCode.Escape         // ESC 键
KeyCode.Tab            // Tab 键
```

## 🎯 完整测试流程

1. **创建 InputSystem**
   ```
   Hierarchy > 右键 > Create Empty > 命名为 "InputSystem"
   拖拽 InputSystem.cs 到这个 GameObject
   设置 Current Input Mode = Keyboard
   勾选 Show Debug Log
   ```

2. **创建测试监听器**
   ```
   Hierarchy > 右键 > Create Empty > 命名为 "TestListener"
   拖拽 InputListener.cs 到这个 GameObject
   勾选 Show Debug Log
   ```

3. **运行测试**
   ```
   点击 Play 按钮
   按下空格键
   查看 Console 是否输出：
   - [InputSystem] 玩家输入 - 模式: Keyboard
   - [InputListener] 收到玩家输入！执行判定逻辑...
   ```

4. **如果没有输出**
   ```
   在 Console 中输入以下命令测试：
   InputSystem.Instance.GetCurrentInputMode()  // 查看当前模式
   InputSystem.Instance.GetKeyboardKey()       // 查看当前按键
   ```

## 💡 提示

- 设置会自动保存到 PlayerPrefs，下次启动游戏会自动加载
- 可以调用 `InputSystem.Instance.ResetInputSettings()` 重置所有设置
- 事件订阅必须在 `OnDisable()` 中取消，否则会导致内存泄漏
- 建议在游戏开始前让玩家选择输入模式
