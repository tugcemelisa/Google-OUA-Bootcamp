
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviourSingletonPersistent<SoundManager>
{
    [Range(0f, 1f)]
    [SerializeField] float soundVolume = .5f;
    [Range(0f, 1f)]
    [SerializeField] float ambientSoundVolume = .5f;
    [SerializeField] List<SoundData> sounds;
    [SerializeField] List<AmbientSoundData> ambientSounds;

    [SerializeField] AudioSource ambientAudioSource;

    [SerializeField] AudioSource[] sources;
    Queue<AudioSource> audioSources;

    private void Start()
    {
        audioSources = new Queue<AudioSource>();
        foreach (var source in sources)
        {
            audioSources.Enqueue(source);
        }
    }

    public void ChangeAmbientSound(VoiceType voice)
    {
        ambientAudioSource.volume = soundVolume;
        foreach (var sound in sounds)
        {
            if (sound.VoiceType == voice)
            {
                ambientAudioSource.clip = sound.Clip;
                ambientAudioSource.Play();
                break;
            }
        }
    }

    public void PlaySound(VoiceType voice, Transform parent, Vector3 pos)
    {
        if (audioSources.TryDequeue(out AudioSource audioSource))
        {
            print("Not enough audio sources");
            return;
        }

        audioSource.volume = soundVolume;
        foreach (var sound in sounds)
        {
            if (sound.VoiceType == voice)
            {

                audioSource.transform.parent = parent;
                audioSource.transform.position = pos;
                audioSource.clip = sound.Clip;
                audioSource.Play();
                StartCoroutine(VoiceFinished(audioSource, sound.Clip.length + .1f));
                break;
            }
        }
    }

    IEnumerator VoiceFinished(AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);

        audioSources.Enqueue(source);
    }

    public void SetAmbientSoundLevel(float volume)
    {
        ambientSoundVolume = volume;
        ambientAudioSource.volume = volume;
    }

    public void SetNormalSoundLevel(float volume)
    {
        soundVolume = volume;
    }
}

public enum AmbientSoundType
{
    NormalVillage,
    NightAmbient,
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
    WolfHurt,

}

[Serializable]
public class SoundData
{
    public VoiceType VoiceType;
    public string Name;
    public AudioClip Clip;
}

[Serializable]
public class AmbientSoundData
{
    public AmbientSoundType VoiceType;
    public string Name;
    public AudioClip Clip;
}
