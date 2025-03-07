using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioManager : MonoBehaviour
{
    public AudioClip lightFootstep;
    public AudioClip bigFootstep;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip gem;
    public AudioClip pageflip;
    public AudioClip wallSlide;

    private AudioSource source;
    private Dictionary<PlayerAudioSignal, AudioTrack> audioStore = new Dictionary<PlayerAudioSignal, AudioTrack>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private class AudioTrack
    {
        public AudioClip clip;
        public bool continuous;
        public AudioTrack(AudioClip clip, bool continuous)
        {
            this.clip = clip;
            this.continuous = continuous;
        }
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        audioStore.Add(PlayerAudioSignal.LIGHT_FOOTSTEP, new AudioTrack(lightFootstep, false));
        audioStore.Add(PlayerAudioSignal.BIG_FOOTSTEP, new AudioTrack(bigFootstep, false));
        audioStore.Add(PlayerAudioSignal.JUMP, new AudioTrack(jump, false));
        audioStore.Add(PlayerAudioSignal.LAND, new AudioTrack(land, false));
        audioStore.Add(PlayerAudioSignal.GEM, new AudioTrack(gem, false));
        audioStore.Add(PlayerAudioSignal.PAGE_FLIP, new AudioTrack(pageflip, false));
        audioStore.Add(PlayerAudioSignal.WALL_SLIDE, new AudioTrack(wallSlide, true));
    }

    public void Signal(PlayerAudioSignal signal)
    {
        AudioTrack track = audioStore[signal];
        if (track.continuous)
        {
            source.clip = track.clip;
            source.Play();
        } else
        {
            source.PlayOneShot(track.clip);
        }
    }

    public void Stop()
    {
        source.clip = null; // don't use audioSource.Stop() because it messes with the OneShots
    }
}

public enum PlayerAudioSignal { LIGHT_FOOTSTEP, BIG_FOOTSTEP, JUMP, LAND, GEM, PAGE_FLIP, WALL_SLIDE }