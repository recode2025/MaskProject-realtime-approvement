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
    [SerializeField] private GameObject Panel;
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
            CoinsCount.text = "гд" + money.ToString();
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
            Panel.SetActive(!Panel.activeInHierarchy);
        });

        BuyBonusLevel.onClick.AddListener(() => {
            
        });

        BuyRateLevel.onClick.AddListener(() => {

        });

        BuySpLevel.onClick.AddListener(() => {

        });

        BuySpecialBonusLevel.onClick.AddListener(() => {

        });
    }

    // Update is called once per frame
    void Update() {

    }
}
