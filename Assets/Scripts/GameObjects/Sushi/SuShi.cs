using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuShi : MonoBehaviour {
    public GameObject rice;
    public GameObject fish;
    public float surviveTime = 8f;
    public event Action OnFishAdded;
    public int type = 0;
    public float bonus = 350;
    public bool hasAdd = false;
    
    private Image cover; // 盖子Image（自动查找）

    // Start is called before the first frame update
    void Start() {
        // 自动查找名为"Cover"的子对象
        Transform coverTransform = transform.Find("Cover");
        if (coverTransform != null) {
            cover = coverTransform.GetComponent<Image>();
            if (cover != null) {
                cover.gameObject.SetActive(false);
            }
            else {
                Debug.LogWarning($"[SuShi] 找到Cover对象但没有Image组件");
            }
        }
        else {
            Debug.LogWarning($"[SuShi] 预制体中找不到名为'Cover'的子对象");
        }
        
        Destroy(gameObject, surviveTime);
    }

    // Update is called once per frame
    void Update() {

    }

    public void DoAddFish() {
        Destroy(fish);
        
        // 显示盖子
        if (cover != null) {
            cover.gameObject.SetActive(true);
        }
    }

    public void AddFish() {
        if (hasAdd) {
            GameManager.Instance.Miss();
            return;
        }
        hasAdd = true;
        DoAddFish();
        
        Debug.Log("Add Fish!");
        OnFishAdded?.Invoke();
    }

    private void OnDestroy() {
        //if (!hasAdd) GameManager.Instance.Miss();
    }
}
