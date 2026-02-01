# 特殊模式视觉控制器使用说明

## 功能概述
监听 GameManager 的特殊模式状态变化，自动切换背景和角色，带渐变效果。

## 特性
- 背景从左到右渐变切换
- 角色立即切换
- 自动监听 GameManager 事件
- 支持双向切换（进入/退出特殊模式）

## 工作原理

### 特殊模式触发条件
根据 GameManager 的逻辑：
- **进入特殊模式**: `specialPoint >= GameBalance.MaxSp`
- **退出特殊模式**: `specialPoint == 0`

### 视觉切换流程

#### 进入特殊模式
1. 立即切换角色（普通 → 特殊）
2. 特殊背景从左到右渐变显现
3. 持续时间：1秒（可调）

#### 退出特殊模式
1. 立即切换角色（特殊 → 普通）
2. 特殊背景从左到右渐变消失
3. 持续时间：1秒（可调）

## Unity 设置步骤

### 1. 准备背景和角色

#### 背景结构
```
Canvas
├── NormalBackground (Image) - 普通背景
└── SpecialBackground (Image) - 特殊背景（叠加在上面）
```

#### 角色结构
```
Canvas
├── NormalCharacter (GameObject) - 普通角色
└── SpecialCharacter (GameObject) - 特殊角色
```

### 2. 创建控制器对象
在 Canvas 下创建空物体，命名为 "SpecialModeController"

### 3. 添加脚本
在 SpecialModeController 上添加 `SpecialModeVisualController` 脚本

### 4. 配置脚本

在 Inspector 中设置：

**背景设置**
- **Normal Background**: 拖入普通背景 Image
- **Special Background**: 拖入特殊背景 Image

**角色设置**
- **Normal Character**: 拖入普通角色 GameObject
- **Special Character**: 拖入特殊角色 GameObject

**渐变设置**
- **Transition Duration**: 渐变时长（默认1秒）

**调试**
- **Show Debug Log**: 勾选后显示详细日志

### 5. 背景 Image 设置

**重要！** 特殊背景 Image 需要设置为 Filled 类型：
1. 选中 SpecialBackground Image
2. 在 Inspector 中：
   - Image Type: Filled
   - Fill Method: Horizontal
   - Fill Origin: Left
   - Fill Amount: 0（初始值）

## 渐变效果说明

### 使用 Image Fill 实现
- **Fill Method**: Horizontal（水平填充）
- **Fill Origin**: Left（从左开始）
- **Fill Amount**: 0 → 1（显现），1 → 0（消失）

### 渐变方向
- **进入特殊模式**: 从左到右显现（0% → 100%）
- **退出特殊模式**: 从左到右消失（100% → 0%）

### 位置不变
- 背景 Image 的位置和大小保持不变
- 只改变 fillAmount 值
- 视觉效果：像窗帘一样从左到右拉开/关闭

## 事件监听

脚本自动监听 GameManager 的事件：
```csharp
GameManager.Instance.OnSpecialModeChanged += OnSpecialModeChanged;
```

当 `isSpecialMode` 变化时自动触发切换。

## 测试方法

### 方法1：通过 GameManager
在游戏中正常触发特殊模式：
1. 增加 specialPoint 到最大值
2. 观察背景和角色切换
3. 等待 specialPoint 降到 0
4. 观察切换回普通模式

### 方法2：手动测试
在脚本中调用测试方法：
```csharp
SpecialModeVisualController controller = GetComponent<SpecialModeVisualController>();

// 测试切换到特殊模式
controller.TestSwitchToSpecial();

// 测试切换回普通模式
controller.TestSwitchToNormal();
```

## 参数调整

### 渐变速度
- **更快**: Transition Duration = 0.5 秒
- **标准**: Transition Duration = 1.0 秒
- **更慢**: Transition Duration = 2.0 秒

### 渐变方向
如果想改变渐变方向，修改 SpecialBackground 的：
- Fill Origin: Right（从右到左）
- Fill Method: Vertical（垂直方向）

## 注意事项

1. **背景层级**: 特殊背景必须在普通背景上面
2. **Image 类型**: 特殊背景必须设置为 Filled 类型
3. **初始状态**: 特殊背景的 Fill Amount 初始为 0
4. **角色切换**: 角色是立即切换，不是渐变
5. **GameManager**: 确保场景中有 GameManager 实例

## 调试技巧

1. 勾选 `Show Debug Log` 查看切换日志
2. 观察 Console 中的状态变化
3. 检查 specialPoint 的值变化
4. 使用测试方法手动触发切换

## 常见问题

**Q: 背景没有渐变效果？**
A: 检查 SpecialBackground 的 Image Type 是否设置为 Filled。

**Q: 角色没有切换？**
A: 检查角色 GameObject 是否正确设置，以及初始状态是否正确。

**Q: 渐变方向不对？**
A: 检查 Fill Origin 设置，Left = 从左到右，Right = 从右到左。

**Q: 没有触发切换？**
A: 检查是否正确订阅了 GameManager 的事件，以及 GameManager 是否存在。

## 扩展功能

### 添加音效
在切换时播放音效：
```csharp
private void OnSpecialModeChanged(bool isSpecial)
{
    // 播放音效
    if (isSpecial)
    {
        AudioSource.PlayClipAtPoint(enterSpecialSound, Camera.main.transform.position);
    }
    else
    {
        AudioSource.PlayClipAtPoint(exitSpecialSound, Camera.main.transform.position);
    }
    
    // ... 原有代码
}
```

### 添加粒子效果
在切换时触发粒子效果：
```csharp
public ParticleSystem specialModeParticles;

private void OnSpecialModeChanged(bool isSpecial)
{
    if (isSpecial && specialModeParticles != null)
    {
        specialModeParticles.Play();
    }
    
    // ... 原有代码
}
```

### 添加角色渐变
如果想让角色也有渐变效果：
```csharp
private IEnumerator TransitionToSpecialMode()
{
    // 角色淡入淡出
    yield return StartCoroutine(FadeOutCharacter(normalCharacter));
    normalCharacter.SetActive(false);
    specialCharacter.SetActive(true);
    yield return StartCoroutine(FadeInCharacter(specialCharacter));
    
    // ... 背景渐变代码
}
```
