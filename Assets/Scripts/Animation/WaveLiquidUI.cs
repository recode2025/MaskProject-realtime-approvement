using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 波浪液体UI组件 - 使用UI Image和Mesh实现波浪效果
/// 完全使用脚本生成，无需额外资源
/// </summary>
[RequireComponent(typeof(CanvasRenderer))]
public class WaveLiquidUI : MaskableGraphic
{
    [Header("液体设置")]
    [SerializeField] private Color _liquidColor = new Color(1f, 0.5f, 0f, 1f);
    [SerializeField] [Range(0f, 1f)] private float _fillAmount = 0f;
    
    [Header("波浪设置")]
    [SerializeField] private float _waveHeight = 20f;
    [SerializeField] private float _waveSpeed = 3f;
    [SerializeField] private int _waveSegments = 50;
    [SerializeField] private float _waveOffset = 0f; // 相位偏移
    
    private float waveTime = 0f;
    
    public Color liquidColor
    {
        get { return _liquidColor; }
        set { _liquidColor = value; SetVerticesDirty(); }
    }
    
    public float fillAmount
    {
        get { return _fillAmount; }
        set 
        { 
            _fillAmount = Mathf.Clamp01(value);
            SetVerticesDirty();
        }
    }
    
    public float waveHeight
    {
        get { return _waveHeight; }
        set { _waveHeight = value; SetVerticesDirty(); }
    }
    
    public float waveSpeed
    {
        get { return _waveSpeed; }
        set { _waveSpeed = value; }
    }
    
    public int waveSegments
    {
        get { return _waveSegments; }
        set { _waveSegments = Mathf.Max(10, value); SetVerticesDirty(); }
    }
    
    public float waveOffset
    {
        get { return _waveOffset; }
        set { _waveOffset = value; SetVerticesDirty(); }
    }
    
    protected override void Awake()
    {
        base.Awake();
        
        // 设置RectTransform
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = Vector2.zero;
    }
    
    void Update()
    {
        if (_fillAmount > 0f && _fillAmount < 1f)
        {
            // 只在填充过程中更新波浪动画
            waveTime += Time.deltaTime * _waveSpeed;
            SetVerticesDirty();
        }
    }
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        if (_fillAmount <= 0f)
        {
            return;
        }
        
        Rect rect = rectTransform.rect;
        float width = rect.width;
        float height = rect.height;
        
        // 计算填充高度
        float fillHeight = height * _fillAmount;
        float baseY = rect.yMin + fillHeight;
        
        // 创建底部矩形（液体主体）
        Vector2 bottomLeft = new Vector2(rect.xMin, rect.yMin);
        Vector2 bottomRight = new Vector2(rect.xMax, rect.yMin);
        
        // 创建波浪顶部
        int segments = _waveSegments;
        float segmentWidth = width / segments;
        
        // 添加底部两个顶点
        vh.AddVert(bottomLeft, _liquidColor, Vector2.zero);
        vh.AddVert(bottomRight, _liquidColor, Vector2.zero);
        
        // 添加波浪顶部顶点
        for (int i = 0; i <= segments; i++)
        {
            float x = rect.xMin + i * segmentWidth;
            float normalizedX = (float)i / segments;
            
            // 计算波浪偏移
            float wave = 0f;
            if (_fillAmount < 1f) // 只在未完全填充时显示波浪
            {
                wave = Mathf.Sin((normalizedX + waveTime + _waveOffset) * Mathf.PI * 2f) * _waveHeight;
            }
            
            float y = baseY + wave;
            
            // 限制在容器内
            y = Mathf.Min(y, rect.yMax);
            
            Vector2 pos = new Vector2(x, y);
            vh.AddVert(pos, _liquidColor, Vector2.zero);
        }
        
        // 创建三角形
        for (int i = 0; i < segments; i++)
        {
            int bottomLeftIndex = 0;
            int bottomRightIndex = 1;
            int topLeftIndex = 2 + i;
            int topRightIndex = 2 + i + 1;
            
            // 第一个三角形
            vh.AddTriangle(bottomLeftIndex, topLeftIndex, topRightIndex);
            // 第二个三角形
            vh.AddTriangle(bottomLeftIndex, topRightIndex, bottomRightIndex);
        }
    }
    
    public override Color color
    {
        get { return _liquidColor; }
        set { _liquidColor = value; SetVerticesDirty(); }
    }
    
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        _fillAmount = Mathf.Clamp01(_fillAmount);
        _waveSegments = Mathf.Max(10, _waveSegments);
    }
#endif
}
