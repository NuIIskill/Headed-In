using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class initMainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string gameMusicTrack = "Game";
    [SerializeField] private string clickSound = "click";

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "GameScene";

    private const string MusicMixerParameter = "MusicVolume";
    private const string SfxMixerParameter = "SFXVolume";
    private const string MusicPrefsKey = "MusicVolumeSlider";
    private const string SfxPrefsKey = "SFXVolumeSlider";
    private const string MenuMusicTrack = "menumusic";

    private void Start()
    {
        SetupSliders();
        LoadVolume();
        PlayMenuMusic();
        OpenMainMenu();
    }

    private void SetupSliders()
    {
        SetupSlider(musicSlider, UpdateMusicVolume);
        SetupSlider(sfxSlider, UpdateSoundVolume);
    }

    private void SetupSlider(Slider slider, UnityEngine.Events.UnityAction<float> callback)
    {
        if (slider == null)
        {
            return;
        }

        slider.minValue = 0f;
        slider.maxValue = 1f;
        slider.wholeNumbers = false;
        slider.onValueChanged.RemoveListener(callback);
        slider.onValueChanged.AddListener(callback);
    }

    public void PlayClick()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound2D(clickSound);
        }
    }

    public void OnStartClick()
    {
        PlayGameMusic();
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenOptions()
    {
        SetMenuState(false, true);
    }

    public void OpenMainMenu()
    {
        SetMenuState(true, false);
    }

    private void SetMenuState(bool showMainMenu, bool showOptionsMenu)
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(showMainMenu);
        }

        if (optionsMenu != null)
        {
            optionsMenu.SetActive(showOptionsMenu);
        }
    }

    public void UpdateMusicVolume(float value)
    {
        SetAndSaveVolume(MusicMixerParameter, MusicPrefsKey, value);
    }

    public void UpdateSoundVolume(float value)
    {
        SetAndSaveVolume(SfxMixerParameter, SfxPrefsKey, value);
    }

    public void LoadVolume()
    {
        float musicValue = NormalizeSliderValue(PlayerPrefs.GetFloat(MusicPrefsKey, 1f));
        float sfxValue = NormalizeSliderValue(PlayerPrefs.GetFloat(SfxPrefsKey, 1f));

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(musicValue);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(sfxValue);
        }

        SetMixerVolume(MusicMixerParameter, musicValue);
        SetMixerVolume(SfxMixerParameter, sfxValue);
    }

    private void SetAndSaveVolume(string mixerParameter, string prefsKey, float value)
    {
        value = NormalizeSliderValue(value);
        SetMixerVolume(mixerParameter, value);
        PlayerPrefs.SetFloat(prefsKey, value);
        PlayerPrefs.Save();
    }

    private void PlayMenuMusic()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic(MenuMusicTrack);
        }
    }

    private void PlayGameMusic()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayMusic(gameMusicTrack);
        }
    }

    private float NormalizeSliderValue(float value)
    {
        if (value < 0f)
        {
            return Mathf.Clamp01(Mathf.Pow(10f, value / 20f));
        }

        return Mathf.Clamp01(value);
    }

    private void SetMixerVolume(string parameterName, float sliderValue)
    {
        if (audioMixer == null)
        {
            return;
        }

        sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float volumeDb = Mathf.Log10(sliderValue) * 20f;
        audioMixer.SetFloat(parameterName, volumeDb);
    }
}
