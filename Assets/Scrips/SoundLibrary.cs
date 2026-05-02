using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}

public class SoundLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffect[] soundEffects;

    public AudioClip GetClip(string groupID)
    {
        if (string.IsNullOrWhiteSpace(groupID) || soundEffects == null)
        {
            return null;
        }

        foreach (SoundEffect soundEffect in soundEffects)
        {
            if (soundEffect.groupID == groupID)
            {
                return GetRandomClip(soundEffect.clips);
            }
        }

        return null;
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
        {
            return null;
        }

        return clips[Random.Range(0, clips.Length)];
    }
}
