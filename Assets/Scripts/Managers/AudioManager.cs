using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public AudioMixerGroup EffGroup, SoundGroup;
    [SerializeField]
    public Audio[] EffectClips, MusicClips;

    AudioSource Effects, Sound;

    private void Start()
    {
        Effects = gameObject.AddComponent<AudioSource>();
        Effects.outputAudioMixerGroup = EffGroup;
        Sound = gameObject.AddComponent<AudioSource>();
        Sound.outputAudioMixerGroup = SoundGroup;
    }

    public void PlayEffect(string EffectName)
    {
        foreach (var item in EffectClips)
        {
            if (item.AudioName == EffectName) Sound.clip = item.Clip;
        }
        Effects.pitch = UnityEngine.Random.Range(.9f, 1.1f);
        Effects.Play();
    }

    public void SetVolume(float Effects, float Sound)
    {
        Mixer.SetFloat("Vol_Effects", Effects * 40 -40);
        Mixer.SetFloat("Vol_Sound", Sound * 40 - 40);
    }
}

[Serializable]
public class Audio
{
    public string AudioName;
    public AudioClip Clip;
}