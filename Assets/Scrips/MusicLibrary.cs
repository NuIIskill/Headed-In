using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip clip;
}

public class MusicLibrary : MonoBehaviour
{
    [SerializeField] private MusicTrack[] tracks;

    public AudioClip GetClip(string trackName)
    {
        if (string.IsNullOrWhiteSpace(trackName) || tracks == null)
        {
            return null;
        }

        foreach (MusicTrack track in tracks)
        {
            if (track.trackName == trackName)
            {
                return track.clip;
            }
        }

        return null;
    }
}
