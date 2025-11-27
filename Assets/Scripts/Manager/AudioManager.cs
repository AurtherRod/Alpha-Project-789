using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] GameObject AudioCanvas;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] AudioClip LoopableMusic;
    [SerializeField] AudioClip ButtonClickSFX;
    [SerializeField] AudioClip WinGameSFX;

    [SerializeField] Slider MusicVolumeSlider;
    [SerializeField] Slider SFXVolumeSlider;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayLoopableMusic();
    }

    public void PlayButtonClickSFX()
    {
        sfxSource.PlayOneShot(ButtonClickSFX);
    }

    public void PlayWinGameSFX()
    {
        sfxSource.PlayOneShot(WinGameSFX);
    }

    public void PlayLoopableMusic()
    {
        musicSource.clip = LoopableMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void AudioPanelClose()
    {
        AudioCanvas.SetActive(false);
    }

    public void AudioPanelOpen()
    {
        AudioCanvas.SetActive(true);
    }

    public void UpdateMusicVolume()
    {
        float volume = MusicVolumeSlider.value;
        audioMixer.SetFloat("MusicVolume", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }

    public void UpdateSFXVolume()
    {
        float volume = SFXVolumeSlider.value;
        audioMixer.SetFloat("SFXVolume", volume > 0 ? Mathf.Log10(volume) * 20 : -80f);
    }



}
