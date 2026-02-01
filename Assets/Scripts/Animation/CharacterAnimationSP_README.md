# CharacterAnimationSP 使用说明

## 概述
CharacterAnimationSP 是 CharacterAnimation 的简化版本，移除了抬手动作，使用更简洁的动画流程。

## 主要区别

### 原版 (CharacterAnimation)
- **帧数**: 4帧
- **动画流程**: 
  - 放手：1 → 1 → 2 → 3 → 4
  - 抬手：4 → 3 → 2 → 1 → 1
- **特性**: 支持动画期间打断并切换方向

### SP版 (CharacterAnimationSP)
- **帧数**: 5帧
- **动画流程**: 1 → 1 → 1 → 2 → 3 → 4 → 4 → 5 → 1
- **特性**: 
  - 无抬手动作
  - 动画播放期间忽略输入
  - 完整循环后回到待机状态

## 动画序列详解

```
帧序列: 1  1  1  2  3  4  4  5  1
索引:   0  0  0  1  2  3  3  4  0
说明:   待 待 待 过 动 动 动 结 回
        机 机 机 渡 作 作 作 束 待
                                  机
```

- **帧1（重复3次）**: 待机状态
- **帧2**: 过渡帧
- **帧3**: 动作中
- **帧4（重复2次）**: 动作持续
- **帧5**: 结束帧
- **回到帧1**: 回到待机

## Unity 设置

### 1. 添加组件
在角色 Image 对象上添加 `CharacterAnimationSP` 组件

### 2. 配置动画帧
在 Inspector 中：
- **Animation Frames**: 拖入5张动画帧图片（按顺序）
  - Frame 0: 待机帧
  - Frame 1: 过渡帧
  - Frame 2: 动作帧
  - Frame 3: 持续帧
  - Frame 4: 结束帧

### 3. 调整播放速度
- **Frame Time**: 每帧持续时间（默认 1/60 秒）
  - 更快：0.01 秒
  - 标准：1/60 秒 ≈ 0.0167 秒
  - 更慢：0.03 秒

### 4. 调试
- **Show Debug Log**: 勾选后在 Console 显示详细日志

## 使用方式

### 自动触发（推荐）
脚本会自动订阅 `InputSystem.OnPlayerInput` 事件，当玩家输入时自动播放动画。

### 手动触发
```csharp
CharacterAnimationSP anim = GetComponent<CharacterAnimationSP>();
anim.TriggerAnimation();
```

### 停止动画
```csharp
CharacterAnimationSP anim = GetComponent<CharacterAnimationSP>();
anim.StopAnimation();
```

### 检查状态
```csharp
CharacterAnimationSP anim = GetComponent<CharacterAnimationSP>();

// 是否正在播放
bool isPlaying = anim.IsPlaying();

// 获取当前帧索引（0-4）
int currentFrame = anim.GetCurrentFrameIndex();
```

## 工作流程

1. **待机状态**: 显示帧1
2. **收到输入**: 检查是否正在播放
3. **开始播放**: 按序列播放 1→1→1→2→3→4→4→5→1
4. **播放期间**: 忽略所有输入
5. **播放完成**: 回到待机状态（帧1）

## 与原版对比

| 特性 | 原版 | SP版 |
|------|------|------|
| 帧数 | 4帧 | 5帧 |
| 动画长度 | 10帧 | 9帧 |
| 抬手动作 | ✓ 有 | ✗ 无 |
| 打断机制 | ✓ 支持 | ✗ 不支持 |
| 循环方式 | 放手→抬手 | 完整循环 |
| 复杂度 | 较高 | 较低 |

## 适用场景

### 使用 SP 版的情况：
- 不需要抬手动作
- 希望动画更简洁
- 不需要中途打断
- 动画流程固定

### 使用原版的情况：
- 需要完整的放手/抬手动作
- 需要支持动画打断
- 需要更复杂的交互

## 注意事项

1. **帧数要求**: 必须提供5张图片
2. **播放期间**: 会忽略所有输入，直到动画完成
3. **输入系统**: 需要 InputSystem 组件存在
4. **帧时间**: 建议使用 1/60 秒以获得流畅效果

## 调试技巧

1. 勾选 `Show Debug Log` 查看详细日志
2. 观察 Console 中的帧序列输出
3. 调整 `Frame Time` 观察动画速度
4. 使用 `TriggerAnimation()` 手动测试

## 常见问题

**Q: 动画不播放？**
A: 检查是否订阅了 InputSystem 事件，以及动画帧是否都已设置。

**Q: 动画太快/太慢？**
A: 调整 `Frame Time` 参数。

**Q: 点击没反应？**
A: 检查是否正在播放动画，播放期间会忽略输入。

**Q: 如何让动画可以打断？**
A: 使用原版 CharacterAnimation，它支持动画打断。

## 示例代码

```csharp
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private CharacterAnimationSP anim;
    
    void Start()
    {
        anim = GetComponent<CharacterAnimationSP>();
    }
    
    void Update()
    {
        // 按空格键触发动画
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!anim.IsPlaying())
            {
                anim.TriggerAnimation();
            }
        }
        
        // 按ESC键停止动画
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            anim.StopAnimation();
        }
    }
}
```
