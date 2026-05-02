using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [FormerlySerializedAs("sfxLibrary")]
    [SerializeField] private SoundLibrary soundLibrary;
    [SerializeField] private AudioSource sfx2DSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound2D(string soundName)
    {
        if (sfx2DSource == null || soundLibrary == null)
        {
            return;
        }

        PlaySound2D(soundLibrary.GetClip(soundName));
    }

    public void PlaySound2D(AudioClip clip)
    {
        if (sfx2DSource == null || clip == null)
        {
            return;
        }

        sfx2DSource.PlayOneShot(clip);
    }

    public void PlaySound3D(string soundName, Vector3 position)
    {
        if (soundLibrary == null)
        {
            return;
        }

        PlaySound3D(soundLibrary.GetClip(soundName), position);
    }

    public void PlaySound3D(AudioClip clip, Vector3 position)
    {
        if (clip == null)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(clip, position);
    }
}
