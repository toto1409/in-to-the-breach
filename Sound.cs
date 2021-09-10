using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioSource As;


    public void SoundPlay(string _name)
    {
        string _path = string.Format("Sound/{0}", _name);
        AudioClip soundclip = Resources.Load<AudioClip>(_path) as AudioClip;
        As.PlayOneShot(soundclip);
    }

    public void SoundPlay(string _name, float voluem)
    {
        string _path = string.Format("Sound/{0}", _name);
        AudioClip soundclip = Resources.Load<AudioClip>(_path) as AudioClip;
        As.PlayOneShot(soundclip, voluem);
    }
}
