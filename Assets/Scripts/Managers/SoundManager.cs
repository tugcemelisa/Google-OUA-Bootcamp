
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [Range(0f, 1f)]
    [SerializeField] float soundVolume = .5f;
    [SerializeField] List<SoundData> sounds;

    [SerializeField] AudioSource audioSource;

    public void PlaySound(VoiceType voice, Vector3 pos)
    {
        audioSource.volume = soundVolume;
        foreach (var sound in sounds)
        {
            if (sound.VoiceType == voice)
            {
                audioSource.transform.position = pos;
                audioSource.clip = sound.Clip;
                audioSource.Play();
            }
        }
    }

}

public enum VoiceType
{
    YasYasGle,
    ShepherdYell,
    Success,
    FarmAmbiance,
    CowNormal,
    CowStrong,
    Sheep,
    TorchLightningShort,
    TorchLightningLong,
    TorchAttack,
    TorchAttack2,
    WolfHowling,
    WolfScaried,
}

[Serializable]
public class SoundData
{
    public VoiceType VoiceType;
    public string Name;
    public AudioClip Clip;
}
