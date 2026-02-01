using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickableCharacterImage : MonoBehaviour
{
    [Header("目标对象")]
    [SerializeField] private GameObject targetObject;
    
    [Header("显示时长")]
    [SerializeField] private float displayDuration = 2f;
    
    private Image characterImage;
    private Button imageButton;
    private bool isClickable = true;

    private void Awake()
    {
        characterImage = GetComponent<Image>();
        imageButton = GetComponent<Button>();
        
        if (imageButton == null)
        {
            imageButton = gameObject.AddComponent<Button>();
        }
        
        imageButton.onClick.AddListener(OnImageClicked);
    }

    private void OnImageClicked()
    {
        if (!isClickable || targetObject == null)
            return;
        
        StartCoroutine(ShowObjectTemporarily());
    }

    private IEnumerator ShowObjectTemporarily()
    {
        // 禁用点击
        // isClickable = false;
        // imageButton.interactable = false;
        
        // 激活目标对象
        targetObject.SetActive(true);
        
        // 等待指定时长
        yield return new WaitForSeconds(displayDuration);
        
        // 隐藏目标对象
        targetObject.SetActive(false);
        
        // 恢复点击
        // isClickable = true;
        // imageButton.interactable = true;
    }

    private void OnDestroy()
    {
        if (imageButton != null)
        {
            imageButton.onClick.RemoveListener(OnImageClicked);
        }
    }
}
