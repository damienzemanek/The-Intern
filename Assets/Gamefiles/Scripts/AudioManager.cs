using System;
using AYellowpaper.SerializedCollections;
using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public enum SourceTrack
    {
        Player,
        Long1,
        Long2,
    }
    
    public enum AudioClipType
    {
        PhoneBuzz,
    }


    [Required] public AudioSource longAudioSource1;
    [Required] public AudioSource longAudioSource2;
    [Required] public AudioSource playerAudioSource;
    
    public SerializedDictionary<AudioClipType, AudioClip> audioClips = new();


    [Button]
    public void InitializeNotInitializedAudioClipTypes()
    {
        foreach (AudioClipType clipType in Enum.GetValues(typeof(AudioClipType)))
            audioClips.TryAdd(clipType, null);
    }

    public static AudioSource UseSourceWithClip(SourceTrack track, AudioClipType clipType, out AudioClip clip)
    {
        clip = Instance.audioClips[clipType];
        
        switch (track)
        {
            case SourceTrack.Player: return Instance.playerAudioSource;
            case SourceTrack.Long1: return Instance.longAudioSource1;
            case SourceTrack.Long2: return Instance.longAudioSource2;
            default: Debug.LogError("Unspported Source Track Type"); break;
        }

        return null;
    }
}
