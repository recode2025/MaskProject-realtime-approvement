using UnityEngine;
using System.Collections;

public class DelayedBackgroundMusic : MonoBehaviour
{
    [Header("背景音乐设置")]
    [SerializeField] private AudioClip backgroundMusic;
    
    [Header("延迟时间（秒）")]
    [SerializeField] private float delayTime = 8f;
    
    [Header("音量设置")]
    [SerializeField] private float volume = 0.5f;
    
    [Header("循环播放")]
    [SerializeField] private bool loop = true;
    
    [Header("淡出时间（秒）")]
    [SerializeField] private float fadeOutDuration = 1f;
    
    private AudioSource audioSource;
    private float originalVolume;
    private bool isFadingOut = false;

    private void Start()
    {
        SetupAudioSource();
        StartCoroutine(PlayMusicAfterDelay());
    }

    private void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.clip = backgroundMusic;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;
        
        originalVolume = volume;
    }

    private IEnumerator PlayMusicAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("背景音乐未设置或AudioSource组件缺失！");
        }
    }

    public void FadeOutMusic()
    {
        if (!isFadingOut && audioSource != null && audioSource.isPlaying)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        isFadingOut = true;
        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
        isFadingOut = false;
    }
}
