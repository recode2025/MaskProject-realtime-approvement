# 角色表情控制器使用说明

## 功能概述
点击角色显示随机表情，带淡入淡出动画效果。

## 特性
- 随机选择表情
- 淡入淡出动画（总时长3秒）
- 点击时中断当前动画并重新播放
- 每次点击随机新表情

## Unity 设置步骤

### 1. 准备表情图片
将所有表情图片放入 `Assets/Resources/Images/CharacterEmoji/` 文件夹

### 2. 创建 UI 结构
```
Canvas
└── Character (角色Image)
    └── Emoji (表情Image)
```

### 3. 配置角色 Image
1. 选择 Character 对象
2. 添加 `CharacterEmojiController` 脚本
3. 在 Inspector 中配置：
   - **Emoji Sprites**: 拖入所有表情图片（可以多选）
   - **Emoji Image**: 拖入 Emoji 子对象
   - **Fade In Duration**: 淡入时长（默认0.5秒）
   - **Display Duration**: 持续显示时长（默认2秒）
   - **Fade Out Duration**: 淡出时长（默认0.5秒）

### 4. 配置表情 Image
1. 选择 Emoji 对象
2. 设置 RectTransform：
   - 位置：角色头顶或旁边
   - 大小：根据需要调整
3. 初始状态会自动隐藏

## 参数说明

### 表情设置
- **Emoji Sprites**: 表情图片数组，支持多个表情
- **Emoji Image**: 显示表情的 Image 组件

### 动画设置
- **Fade In Duration**: 淡入时长（秒）
- **Display Duration**: 持续显示时长（秒）
- **Fade Out Duration**: 淡出时长（秒）
- **总时长** = 淡入 + 持续 + 淡出（默认3秒）

### 调试
- **Show Debug Log**: 勾选后在 Console 显示详细日志

## 工作流程

1. **点击角色**
2. **停止当前动画**（如果有）
3. **随机选择表情**
4. **淡入动画**（0.5秒）
5. **持续显示**（2秒）
6. **淡出动画**（0.5秒）
7. **隐藏表情**

## 动画细节

### 淡入（Fade In）
- 从完全透明（alpha = 0）到完全不透明（alpha = 1）
- 使用线性插值平滑过渡

### 持续显示（Display）
- 保持完全不透明状态
- 等待指定时长

### 淡出（Fade Out）
- 从完全不透明（alpha = 1）到完全透明（alpha = 0）
- 使用线性插值平滑过渡

## 中断机制

当动画播放期间再次点击：
1. 立即停止当前协程
2. 重新随机选择表情
3. 从头开始播放新动画

这确保了每次点击都能看到新的表情。

## 示例配置

### 快速表情（1.5秒总时长）
- Fade In Duration: 0.3
- Display Duration: 0.9
- Fade Out Duration: 0.3

### 标准表情（3秒总时长）
- Fade In Duration: 0.5
- Display Duration: 2.0
- Fade Out Duration: 0.5

### 慢速表情（5秒总时长）
- Fade In Duration: 1.0
- Display Duration: 3.0
- Fade Out Duration: 1.0

## 注意事项

1. **表情数量**: 至少添加1个表情图片
2. **Image 组件**: Emoji 对象必须有 Image 组件
3. **Raycast Target**: Character Image 的 Raycast Target 必须勾选
4. **层级关系**: Emoji 必须是 Character 的子对象或在同一 Canvas 下
5. **透明度**: 确保表情图片支持透明通道（PNG格式）

## 扩展功能

### 添加音效
在 `OnPointerClick` 方法中添加：
```csharp
AudioSource.PlayClipAtPoint(emojiSound, Camera.main.transform.position);
```

### 添加位置偏移
在 `PlayEmojiAnimation` 开始时添加：
```csharp
emojiImage.rectTransform.anchoredPosition = new Vector2(Random.Range(-50, 50), Random.Range(50, 100));
```

### 添加缩放动画
在淡入淡出的同时调整 scale：
```csharp
emojiImage.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
```

## 调试技巧

1. 勾选 `Show Debug Log` 查看详细日志
2. 检查 Console 中的表情索引
3. 确认动画是否被正确中断
4. 验证表情图片是否正确加载

## 常见问题

**Q: 点击没有反应？**
A: 检查 Character Image 的 Raycast Target 是否勾选。

**Q: 表情不显示？**
A: 确认 Emoji Sprites 数组中有图片，且 Emoji Image 已设置。

**Q: 动画不流畅？**
A: 调整淡入淡出时长，或检查帧率是否稳定。

**Q: 表情位置不对？**
A: 调整 Emoji Image 的 RectTransform 位置和锚点。
