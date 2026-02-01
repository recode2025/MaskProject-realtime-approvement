using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SushiSpawner : MonoBehaviour {

    public bool isOn = false;
    public float minTime = 1;
    public float maxTime = 2;
    public float targetTime = 0;
    public float curTime = 0;
    public float originSpeed = 564f;
    public float speedScale = 1.0f;
    public float _scale = 1.0f; // sppedScale畜生被绿
    [SerializeField] private SuShi suShiPrefab;
    [SerializeField] private List<Sprite> fishSprites;
    public GameObject belt;
    
    [Header("寿司大小设置")]
    [Tooltip("寿司的缩放比例")]
    public float sushiScale = 2.0f;
    
    [Header("盘子设置")]
    [Tooltip("盘子贴图")]
    public Sprite plateSprite;
    
    [Tooltip("盘子的缩放比例")]
    public Vector2 plateScale = new Vector2(1.0f, 1.0f);
    
    [Tooltip("盘子相对于寿司的位置偏移")]
    public Vector2 plateOffset = new Vector2(0, -20f);

    private void Awake() {
        // 根据屏幕分辨率设置_scale
        int screenWidth = Screen.width;
        if (screenWidth >= 2560) {
            _scale = 0.8173f;
        } else {
            _scale = 1.0f; // 1920或其他分辨率默认1.0f
        }
        Animator beletAnimator = belt.GetComponent<Animator>();
        beletAnimator.speed = speedScale * _scale;
        
        Debug.Log($"[SushiSpawner] 屏幕宽度: {screenWidth}, _scale: {_scale}");
        
        GameManager.Instance.sushiSpawner = this;
        GameManager.Instance.OnSpecialModeChanged += () => {
            if (GameManager.Instance.isSpecialMode) {
                speedScale = 1.25f;
            }
            else {
                speedScale = 1.0f;
            }
            Animator beletAnimator = belt.GetComponent<Animator>();
            beletAnimator.speed = speedScale * _scale;
        };
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        if (isOn) {
            if (curTime >= targetTime) {
                targetTime = UnityEngine.Random.Range(minTime, maxTime);
                curTime = 0;
                SpawnSushi();
            }
            curTime += Time.fixedDeltaTime;
        }
    }

    protected void SpawnSushi() {
        SuShi sushi = Instantiate<SuShi>(suShiPrefab, this.transform);
        SetSushiType(sushi, GameManager.Instance.GetSuShiType());
        
        // 设置寿司大小
        sushi.transform.localScale = Vector3.one * sushiScale;
        
        // 添加盘子
        if (plateSprite != null)
        {
            CreatePlate(sushi);
        }
        
        Debug.Log($"sushi Type = {sushi.type}");
        sushi.GetComponent<Rigidbody2D>().velocity = Vector2.right * originSpeed * speedScale;
        sushi.OnFishAdded += () => {
            GameManager.Instance.Success(sushi.bonus);
            Debug.Log("cur sushiCount = " + GameManager.Instance.sushiCount);
        };
    }

    protected void SetSushiType(SuShi sushi, int type) {
        sushi.type = type;
        if (type < this.fishSprites.Count && this.fishSprites[type] != null) sushi.rice.GetComponent<Image>().sprite = this.fishSprites[type];
        sushi.bonus = GameManager.Instance.GetBonus(type);
    }
    
    /// <summary>
    /// 创建盘子
    /// </summary>
    protected void CreatePlate(SuShi sushi)
    {
        // 创建盘子GameObject
        GameObject plate = new GameObject("Plate");
        plate.transform.SetParent(sushi.transform, false);
        
        // 添加Image组件
        Image plateImage = plate.AddComponent<Image>();
        plateImage.sprite = plateSprite;
        
        // 获取RectTransform
        RectTransform plateRect = plate.GetComponent<RectTransform>();
        
        // 设置锚点和轴心点为中心
        plateRect.anchorMin = new Vector2(0.5f, 0.5f);
        plateRect.anchorMax = new Vector2(0.5f, 0.5f);
        plateRect.pivot = new Vector2(0.5f, 0.5f);
        
        // 设置位置偏移
        plateRect.anchoredPosition = plateOffset;
        
        // 设置缩放
        plateRect.localScale = new Vector3(plateScale.x, plateScale.y, 1f);
        
        // 设置层级：将盘子移到最前面（第一个子对象），这样它会在寿司下面渲染
        plate.transform.SetAsFirstSibling();
        
        Debug.Log($"[SushiSpawner] 为寿司创建了盘子，位置偏移: {plateOffset}, 缩放: {plateScale}");
    }
}
