using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Ctrl ctrl_;

    public AudioClip cursor;
    public AudioClip drop;
    public AudioClip ctrl;
    public AudioClip clearLine;

    private AudioSource audioSource;

    private bool isMute = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ctrl_ = GetComponent<Ctrl>();
    }

    public void PlayCursor()
    {
        PlayAudio(cursor);
    }

    public void PlayDrop()
    {
        PlayAudio(drop);
    }

    public void PlayControl()
    {
        PlayAudio(ctrl);
    }

    public void PlayClearLine()
    {
        PlayAudio(clearLine);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (isMute) return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void OnAudioButtonClick()
    {
        isMute = !isMute;
        ctrl_.view.SetMuteActive(isMute);
        if (!isMute) PlayCursor();
    }
}
