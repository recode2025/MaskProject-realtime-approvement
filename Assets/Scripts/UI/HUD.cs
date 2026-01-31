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

    // Start is called before the first frame update
    void Start() {
        GameManager.Instance.OnComboUpdated += (int combo) => {
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

        GameManager.Instance.OnSpecialPointUpdated += (float specialPoint) => {
            SpBar.size = specialPoint / GameBalance.MaxSp;
        };

        GameManager.Instance.OnMoneyUpdated += (int money) => {
            CoinsCount.text = money.ToString();
        };

        PauseButton.onClick.AddListener(() => {
            Panel.SetActive(!Panel.activeInHierarchy);
        });
    }

    // Update is called once per frame
    void Update() {

    }
}
