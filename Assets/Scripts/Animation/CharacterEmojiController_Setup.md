# 角色表情系统 - 快速设置指南

## 问题：点击没有反应？

请按照以下步骤检查：

## ✅ 检查清单

### 1. 角色立绘 Image 设置
- [ ] 找到角色立绘的 Image 对象（例如：Character、Girl、Boy 等）
- [ ] 选中该对象，在 Inspector 中查看 Image 组件
- [ ] **重要！** 确保 `Raycast Target` 已勾选 ✓
- [ ] 添加 `CharacterEmojiController` 脚本到这个对象上

### 2. 表情 Image 设置
- [ ] 创建一个新的 Image 对象作为表情显示（例如：Emoji）
- [ ] 可以是角色的子对象，也可以是同级对象
- [ ] 调整位置到角色头顶或旁边

### 3. 脚本配置
在 `CharacterEmojiController` 组件中：
- [ ] **Emoji Sprites**: 拖入所有表情图片（可以多选）
- [ ] **Emoji Image**: 拖入刚创建的表情 Image 对象
- [ ] **Show Debug Log**: 勾选（用于调试）

### 4. 表情图片准备
- [ ] 准备多个表情图片（PNG 格式，支持透明）
- [ ] 放在 `Assets/Resources/Images/CharacterEmoji/` 文件夹
- [ ] 在 Unity 中选中所有表情图片
- [ ] 拖入到 `Emoji Sprites` 数组中

## 🎯 完整设置示例

```
Canvas
├── Character (Image) ← 添加 CharacterEmojiController 脚本
│   └── Emoji (Image) ← 用于显示表情
```

### Character 对象配置：
1. **Image 组件**
   - Source Image: 角色立绘
   - Raycast Target: ✓ 勾选
   
2. **CharacterEmojiController 组件**
   - Emoji Sprites: [表情1, 表情2, 表情3, ...]
   - Emoji Image: Emoji 对象
   - Fade In Duration: 0.5
   - Display Duration: 2.0
   - Fade Out Duration: 0.5
   - Show Debug Log: ✓ 勾选

### Emoji 对象配置：
1. **RectTransform**
   - Position: 根据需要调整（例如：角色头顶）
   - Width: 100
   - Height: 100
   
2. **Image 组件**
   - Source Image: 可以留空（脚本会自动设置）
   - Raycast Target: 不需要勾选

## 🔍 调试步骤

### 第一步：检查是否能检测到点击
1. 勾选 `Show Debug Log`
2. 运行游戏
3. 点击角色立绘
4. 查看 Console，应该看到：`[CharacterEmojiController] 检测到点击 XXX`

**如果没有看到这条日志：**
- 检查角色 Image 的 `Raycast Target` 是否勾选
- 检查是否有其他 UI 挡在角色前面
- 检查 Canvas 的 Graphic Raycaster 组件是否存在

### 第二步：检查配置是否完整
如果看到点击日志，但没有表情显示，检查 Console 是否有警告：
- `emojiImage 未设置！` → 需要拖入表情 Image
- `表情列表为空！` → 需要添加表情图片到数组
- `无法显示表情：配置不完整` → 检查上述两项

### 第三步：检查表情是否显示
如果配置都正确，应该看到：
1. `[CharacterEmojiController] 显示表情 #X`
2. 表情 Image 应该出现并淡入
3. 持续显示 2 秒
4. 淡出并消失
5. `[CharacterEmojiController] 表情动画完成`

## 🚨 常见错误

### 错误1：点击没反应
**原因**: Raycast Target 未勾选
**解决**: 选中角色 Image，勾选 Raycast Target

### 错误2：表情不显示
**原因**: Emoji Image 未设置或表情列表为空
**解决**: 
1. 创建表情 Image 对象
2. 拖入到 Emoji Image 字段
3. 添加表情图片到 Emoji Sprites 数组

### 错误3：表情位置不对
**原因**: Emoji Image 的位置没有调整
**解决**: 调整 Emoji 的 RectTransform Position

### 错误4：表情一直显示
**原因**: 初始状态没有隐藏
**解决**: 脚本会自动隐藏，检查是否有其他脚本干扰

## 💡 快速测试

1. 创建一个简单的测试场景
2. 添加 Canvas
3. 在 Canvas 下添加 Image（角色）
4. 在角色下添加 Image（表情）
5. 按照上述步骤配置
6. 运行并点击测试

## 📞 需要帮助？

如果按照上述步骤仍然无法工作，请提供：
1. Console 中的完整日志
2. 角色对象的 Inspector 截图
3. CharacterEmojiController 组件的配置截图
