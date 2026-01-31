# 液体填充菜单系统使用说明

## 概述
这是一个完全使用脚本实现的液体填充菜单动画系统，无需任何额外的图片或动画资源。实现了MG风格的快速橙色液体填充效果，带有波浪动画。

## 核心组件

### 1. WaveLiquidUI.cs
- 继承自 `MaskableGraphic`，使用 Unity UI 系统
- 通过程序化生成 Mesh 实现波浪效果
- 支持实时动画和填充控制

### 2. LiquidController.cs
- 控制整个液体填充动画流程
- 自动创建双层波浪效果（深橙色和浅橙色）
- 管理菜单面板的淡入淡出

### 3. MenuTrigger.cs
- 简单的触发器脚本
- 可绑定到按钮或其他UI元素
- 支持打开、关闭和切换功能

## 快速设置步骤

### 方法一：完全自动设置（推荐）

1. **创建Canvas**
   - 在 Hierarchy 中右键 → UI → Canvas
   - 设置 Canvas 的 Render Mode 为 Screen Space - Overlay

2. **创建菜单面板**
   - 在 Canvas 下创建一个 Panel（右键 → UI → Panel）
   - 重命名为 "MenuPanel"
   - 添加 CanvasGroup 组件（Add Component → Canvas Group）
   - 在这个 Panel 中添加你的菜单内容（按钮、文字等）

3. **创建液体控制器**
   - 在 Canvas 下创建一个空物体（右键 → Create Empty）
   - 重命名为 "LiquidMenuController"
   - 添加 LiquidController 组件
   - 将 MenuPanel 拖到 LiquidController 的 Menu Panel 字段

4. **创建触发按钮**
   - 在 Canvas 下创建一个 Button（右键 → UI → Button）
   - 重命名为 "OpenMenuButton"
   - 添加 MenuTrigger 组件
   - MenuTrigger 会自动找到 LiquidController

5. **完成！**
   - 运行游戏，点击按钮即可看到液体填充效果

### 方法二：手动设置

如果需要更多控制，可以手动设置：

1. 创建 Canvas 和 MenuPanel（同上）

2. 创建液体容器：
   - 在 Canvas 下创建空物体 "LiquidMenuController"
   - 添加 LiquidController 组件
   - 在 LiquidController 下手动创建：
     - 空物体 "LiquidContainer"（RectTransform 铺满）
     - 在 LiquidContainer 下创建两个空物体：
       - "BackWave" - 添加 WaveLiquidUI 组件
       - "FrontWave" - 添加 WaveLiquidUI 组件

3. 配置 LiquidController：
   - Liquid Container: 拖入 LiquidContainer
   - Menu Panel: 拖入 MenuPanel 的 CanvasGroup
   - 调整颜色和动画参数

## 参数说明

### LiquidController 参数

**组件引用：**
- `Liquid Container`: 液体容器（自动创建时可留空）
- `Menu Panel`: 要显示的菜单面板的 CanvasGroup

**液体颜色：**
- `Back Wave Color`: 后层深橙色（默认：RGB 255, 128, 0）
- `Front Wave Color`: 前层浅橙色（默认：RGB 255, 166, 51）

**动画设置：**
- `Duration`: 填充总时长（默认 0.8秒，MG风格快速）
- `Fill Curve`: 填充曲线（控制加速度）
- `Front Wave Delay`: 前层波浪延迟（默认 0.05秒）
- `Menu Fade Start Time`: 菜单淡入开始时间（0-1，默认 0.7）
- `Menu Fade Duration`: 菜单淡入持续时间（默认 0.3秒）

**波浪设置：**
- `Wave Height`: 波浪高度（默认 20）
- `Wave Speed`: 波浪速度（默认 3）
- `Wave Segments`: 波浪分段数（默认 50，越多越平滑）

### WaveLiquidUI 参数

- `Liquid Color`: 液体颜色
- `Fill Amount`: 填充量（0-1）
- `Wave Height`: 波浪高度
- `Wave Speed`: 波浪动画速度
- `Wave Segments`: 波浪平滑度
- `Wave Offset`: 相位偏移（用于双层波浪错开）

## 使用方法

### 通过代码调用

```csharp
// 获取控制器
LiquidController liquidController = GetComponent<LiquidController>();

// 播放打开动画
liquidController.PlayOpenAnimation();

// 播放关闭动画
liquidController.PlayCloseAnimation();

// 立即设置为打开状态（无动画）
liquidController.SetOpen();

// 立即设置为关闭状态（无动画）
liquidController.SetClosed();

// 检查是否正在播放动画
bool isPlaying = liquidController.IsPlaying();
```

### 通过 MenuTrigger

```csharp
MenuTrigger trigger = GetComponent<MenuTrigger>();

// 打开菜单
trigger.OpenMenu();

// 关闭菜单
trigger.CloseMenu();

// 切换菜单状态
trigger.ToggleMenu();
```

### 通过 Unity 事件

在 Button 的 OnClick 事件中：
1. 点击 + 号添加事件
2. 拖入带有 MenuTrigger 的物体
3. 选择 MenuTrigger → OpenMenu()

## 自定义调整

### 修改颜色
在 LiquidController 中修改：
- `Back Wave Color`: 深色层
- `Front Wave Color`: 浅色层

推荐配色：
- 橙色：(255, 128, 0) 和 (255, 166, 51)
- 蓝色：(0, 128, 255) 和 (51, 166, 255)
- 绿色：(0, 200, 100) 和 (51, 220, 130)
- 紫色：(150, 50, 200) 和 (180, 100, 220)

### 调整速度
- 减小 `Duration` 可以让动画更快（MG风格建议 0.5-1.0秒）
- 增加 `Wave Speed` 可以让波浪动得更快
- 调整 `Fill Curve` 可以改变加速度曲线

### 调整波浪效果
- 增加 `Wave Height` 让波浪更明显
- 增加 `Wave Segments` 让波浪更平滑（但会增加性能消耗）
- 调整 `Front Wave Delay` 改变双层波浪的错开时间

## 性能优化

1. **波浪分段数**：默认 50 已经足够平滑，除非特殊需求不建议增加
2. **动画完成后**：波浪会自动停止更新，不会持续消耗性能
3. **多个菜单**：每个菜单可以有独立的 LiquidController

## 常见问题

### Q: 看不到液体效果？
A: 检查：
- Canvas 是否正确设置
- LiquidController 是否在 MenuPanel 之前（Hierarchy 顺序）
- MenuPanel 是否有 CanvasGroup 组件

### Q: 波浪不平滑？
A: 增加 `Wave Segments` 参数（建议 50-100）

### Q: 动画太慢/太快？
A: 调整 `Duration` 参数，MG风格建议 0.5-1.0 秒

### Q: 颜色不对？
A: 在 LiquidController 中修改 `Back Wave Color` 和 `Front Wave Color`

### Q: 菜单内容被液体遮挡？
A: 确保 MenuPanel 在 Hierarchy 中位于 LiquidContainer 之后

## 技术细节

### 工作原理
1. `WaveLiquidUI` 继承自 `MaskableGraphic`，使用 Unity UI 的渲染系统
2. 通过 `OnPopulateMesh` 方法程序化生成波浪形状的 Mesh
3. 使用正弦函数计算波浪的起伏
4. 通过 `fillAmount` 控制液体高度
5. 双层波浪通过不同的速度和相位偏移产生层次感

### 优势
- 完全使用脚本，无需任何图片资源
- 性能优秀，使用 Unity UI 原生渲染
- 高度可定制，所有参数都可调整
- 支持任意分辨率和屏幕尺寸
- 与 Unity UI 系统完美集成

## 扩展建议

1. **添加音效**：在动画开始/结束时播放音效
2. **添加粒子效果**：在液体顶部添加气泡粒子
3. **多种液体类型**：创建不同颜色的预设
4. **交互反馈**：添加按钮悬停效果
5. **关闭动画**：实现液体下降的关闭效果

## 示例场景结构

```
Canvas
├── LiquidMenuController (LiquidController)
│   └── LiquidContainer (自动创建)
│       ├── BackWave (WaveLiquidUI)
│       └── FrontWave (WaveLiquidUI)
├── MenuPanel (CanvasGroup)
│   ├── Title (Text)
│   ├── Button1
│   ├── Button2
│   └── CloseButton (MenuTrigger - CloseMenu)
└── OpenMenuButton (Button + MenuTrigger)
```

## 版本信息
- 版本：1.0
- Unity 版本要求：2019.4 或更高
- 依赖：Unity UI (UnityEngine.UI)
