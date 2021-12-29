using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;

    private void Awake() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volumn;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null) s.source.Play();
        else Debug.Log("Can't find the audio clip : " + name);
    }


    public void Stop(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null) s.source.Stop();
        else Debug.Log("Can't find the audio clip : " + name);
    }
}
