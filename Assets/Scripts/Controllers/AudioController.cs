using System;
using System.Linq;
using UnityEngine;

using Utilsf;

[Serializable] public class Sound {
    public string tag;
    public AudioClip clip;

    [Range(0, 1)] public float volume = 1f;
    public bool loop = false;

    [HideInInspector] public AudioSource source;
}

public class AudioController : MonoBehaviour {
    #region editor
    [SerializeField] private bool on = true;
    [SerializeField] private Sound[] sounds = default;
    #endregion

    #region private
    private void Awake() {
        foreach (Sound s in sounds) {
            GameObject sourceObject = new GameObject(s.tag);
            AudioSource source = sourceObject.AddComponent<AudioSource>();
            Utility.parentTo(sourceObject, gameObject);

            s.source = source;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }
    #endregion

    #region public
    public void play(string tag) {
        if (!on) return;
        Sound sound = sounds.FirstOrDefault(s => s.tag == tag);
        if (sound != null) sound.source.Play();
    }

    public void stop(string tag) {
        if (!on) return;
        Sound sound = sounds.FirstOrDefault(s => s.tag == tag);
        if (sound != null) sound.source.Stop();
    }

    public void playRandomBackground() {
        stop("background_1");
        stop("background_2");

        int rnd = UnityEngine.Random.Range(1, 3);
        string tag = $"background_{rnd}";
        play(tag);
    }

    //public void musicOn() {
    //    if (!on) return;
    //    sounds.First(s => s.tag == "background").source.Play();
    //}

    //public void musicOff() {
    //    if (!on) return;
    //    sounds.First(s => s.tag == "background").source.Stop();
    //}
    #endregion
}
