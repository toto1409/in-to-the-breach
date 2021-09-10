using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsSound : MonoBehaviour
{
    public AudioSource As;

    void Start()
    {
        As = GameObject.Find("GameSystem").GetComponent<AudioSource>();
    }

    public void SoundPlay(string _name)
    {
        string _path = string.Format("Sound/{0}", _name);
        AudioClip soundclip = Resources.Load<AudioClip>(_path) as AudioClip;
        As.PlayOneShot(soundclip);
    }

    public void SoundPlayVolMax(string _name)
    {
        string _path = string.Format("Sound/{0}", _name);
        AudioClip soundclip = Resources.Load<AudioClip>(_path) as AudioClip;
        As.PlayOneShot(soundclip, 1f);
    }
}
