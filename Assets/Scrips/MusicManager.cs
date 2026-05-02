using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float defaultFadeDuration = 0.5f;

    private const string MenuMusicTrackName = "menumusic";
    private const string LegacyMenuMusicTrackName = "MainMenu";

    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicLibrary == null)
        {
            musicLibrary = GetComponent<MusicLibrary>();
        }

        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        if (musicSource == null)
        {
            musicSource = GetComponentInChildren<AudioSource>();
        }
    }

    public void PlayMusic(string trackName)
    {
        PlayMusic(trackName, defaultFadeDuration);
    }

    public void PlayMusic(string trackName, float fadeDuration)
    {
        if (musicLibrary == null || musicSource == null)
        {
            Debug.LogWarning("MusicManager is missing a MusicLibrary or AudioSource reference.", this);
            return;
        }

        trackName = NormalizeTrackName(trackName);
        AudioClip nextTrack = musicLibrary.GetClip(trackName);

        if (nextTrack == null)
        {
            Debug.LogWarning($"Music track '{trackName}' was not found in the MusicLibrary.", this);
            return;
        }

        PlayMusic(nextTrack, fadeDuration);
    }

    private string NormalizeTrackName(string trackName)
    {
        if (trackName == LegacyMenuMusicTrackName)
        {
            return MenuMusicTrackName;
        }

        return trackName;
    }

    public void PlayMusic(AudioClip nextTrack)
    {
        PlayMusic(nextTrack, defaultFadeDuration);
    }

    public void PlayMusic(AudioClip nextTrack, float fadeDuration)
    {
        if (musicSource == null || nextTrack == null)
        {
            return;
        }

        if (musicSource.clip == nextTrack && musicSource.isPlaying)
        {
            return;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(Crossfade(nextTrack, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 0.5f)
    {
        if (musicSource == null)
        {
            return;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeOutAndStop(fadeDuration));
    }

    private IEnumerator Crossfade(AudioClip nextTrack, float fadeDuration)
    {
        fadeDuration = Mathf.Max(0f, fadeDuration);

        if (fadeDuration <= 0f)
        {
            musicSource.clip = nextTrack;
            musicSource.volume = 1f;
            musicSource.Play();
            fadeRoutine = null;
            yield break;
        }

        if (nextTrack.loadState == AudioDataLoadState.Unloaded)
        {
            nextTrack.LoadAudioData();
        }

        while (nextTrack.loadState == AudioDataLoadState.Loading)
        {
            yield return null;
        }

        if (nextTrack.loadState == AudioDataLoadState.Failed)
        {
            Debug.LogWarning($"Music track '{nextTrack.name}' failed to load.", this);
            fadeRoutine = null;
            yield break;
        }

        if (!musicSource.isPlaying || musicSource.clip == null)
        {
            musicSource.clip = nextTrack;
            musicSource.volume = 0f;
            musicSource.Play();

            float fadeInTimer = 0f;

            while (fadeInTimer < fadeDuration)
            {
                fadeInTimer += Time.unscaledDeltaTime;
                musicSource.volume = Mathf.Lerp(0f, 1f, fadeInTimer / fadeDuration);
                yield return null;
            }

            musicSource.volume = 1f;
            fadeRoutine = null;
            yield break;
        }

        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        musicSource.volume = 1f;
        fadeRoutine = null;
    }

    private IEnumerator FadeOutAndStop(float fadeDuration)
    {
        fadeDuration = Mathf.Max(0f, fadeDuration);

        if (fadeDuration <= 0f)
        {
            musicSource.Stop();
            musicSource.clip = null;
            musicSource.volume = 1f;
            fadeRoutine = null;
            yield break;
        }

        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = null;
        musicSource.volume = 1f;
        fadeRoutine = null;
    }
}
