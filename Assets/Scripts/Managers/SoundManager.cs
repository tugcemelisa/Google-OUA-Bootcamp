
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [Range(0f, 1f)]
    [SerializeField] float soundVolume = .5f;
    [SerializeField] List<SoundData> sounds;

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

    public void PlaySound(VoiceType voice, Transform parent, Vector3 pos)
    {
        var audioSource = audioSources.Dequeue();
        if (audioSource == null)
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
            }
        }
    }

    IEnumerator VoiceFinished(AudioSource source, float time)
    {
        yield return new WaitForSeconds(time);

        audioSources.Enqueue(source);
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
