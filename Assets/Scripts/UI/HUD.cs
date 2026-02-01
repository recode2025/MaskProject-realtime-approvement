using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField] private Text CoinsCount;
    [SerializeField] private Scrollbar SpBar;
    [SerializeField] private Text ComboCount;
    [SerializeField] private Text Combo;
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button StoreButton; // 新增：商店按钮
    
    [SerializeField] private GameObject Panel; // 恢复：用于自动注册给 GameManager

    [Header("Shop UI")]
    [SerializeField] private Button BuyBonusLevel;
    [SerializeField] private Button BuyRateLevel;
    [SerializeField] private Button BuySpLevel;
    [SerializeField] private Button BuySpecialBonusLevel;
    [SerializeField] private Text BonusPrice;
    [SerializeField] private Text RatePrice;
    [SerializeField] private Text SpPrice;
    [SerializeField] private Text SpecialBonusPrice;
    [SerializeField] private Text BonusLevel;
    [SerializeField] private Text RateLevel;
    [SerializeField] private Text SpLevel;
    [SerializeField] private Text SpecialBonusLevel;

    // Start is called before the first frame update
    void Start() {
        // 自动将 HUD 里的 Panel 注册给 GameManager
        // 这样即使 GameManager 是跨场景保留的旧单例，也能找到当前场景的面板
        if (Panel != null) {
            GameManager.Instance.pausePanel = Panel;
            Panel.SetActive(false); // 确保初始是隐藏的
        } else {
            Debug.LogError("HUD: 请在 Inspector 中将 PausePanel 拖给 HUD 的 Panel 槽位！");
        }

        GameManager.Instance.OnComboChanged += (int combo) => {
            if (combo != 0) {
                ComboCount.gameObject.SetActive(true);
                Combo.gameObject.SetActive(true);
                ComboCount.text = $"x{combo}";
            }
            else {
                ComboCount.gameObject.SetActive(false);
                Combo.gameObject.SetActive(false);
            }
        };

        GameManager.Instance.OnSpecialPointChanged += (float specialPoint) => {
            SpBar.size = specialPoint / GameBalance.MaxSp;
        };

        GameManager.Instance.OnMoneyChanged += (int money) => {
             // 修复合并产生的乱码，暂时只显示数字
            CoinsCount.text = money.ToString();
        };

        GameManager.Instance.OnBonusLevelChanged += (int bonusLevel) => {
            BonusLevel.text = bonusLevel.ToString();
        };

        GameManager.Instance.OnRateLevelChanged += (int rateLevel) => {
            RateLevel.text = rateLevel.ToString();
        };

        GameManager.Instance.OnSpLevelChanged += (int spLevel) => {
            SpLevel.text = spLevel.ToString();
        };

        GameManager.Instance.OnSpecialBonusLevelChanged += (int specialBonusLevel) => {
            SpecialBonusLevel.text = specialBonusLevel.ToString();
        };

        PauseButton.onClick.AddListener(() => {
            // 强制调用 PauseGame 而不是 TogglePause
            // 这样即使 Inspector 和代码双重绑定，也只会执行多次“暂停”，而不会导致“暂停又恢复”
            GameManager.Instance.PauseGame();
        });

        // 绑定商店按钮
        if (StoreButton != null) {
            StoreButton.onClick.AddListener(() => {
                GameManager.Instance.OpenShop();
            });
        }

        // 监听暂停状态，暂停时隐藏按钮，恢复时显示
        GameManager.Instance.OnPauseStateChanged += OnPauseStateChanged;

        // 绑定商店按钮事件 (防止空引用报错)
        if (BuyBonusLevel != null) BuyBonusLevel.onClick.AddListener(() => { });
        if (BuyRateLevel != null) BuyRateLevel.onClick.AddListener(() => { });
        if (BuySpLevel != null) BuySpLevel.onClick.AddListener(() => { });
        if (BuySpecialBonusLevel != null) BuySpecialBonusLevel.onClick.AddListener(() => { });
    }

    private void OnPauseStateChanged(bool isPaused) {
        if (PauseButton != null) {
            PauseButton.gameObject.SetActive(!isPaused);
        }
        if (StoreButton != null) {
            StoreButton.gameObject.SetActive(!isPaused);
        }
    }

    private void OnDestroy() {
        if (GameManager.Instance != null) {
            GameManager.Instance.OnPauseStateChanged -= OnPauseStateChanged;
        }
    }

    // Update is called once per frame
    void Update() {
        // 自动修复 Canvas 丢失 Camera 的问题 (防止 DontDestroyOnLoad 后 UI 消失)
        // 只有当 Canvas 模式为 ScreenSpace - Camera 时才需要
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null) {
            if (Camera.main != null) {
                canvas.worldCamera = Camera.main;
            }
        }
    }
}
