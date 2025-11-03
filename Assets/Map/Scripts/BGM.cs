using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;
    public AudioSource source;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.2f;
        if (intro != null) {
            playIntro();
        } else {
            playLoop();
        }
    }

    void playIntro() {
        source.clip = intro;
        source.loop = false;
        source.Play();
        StartCoroutine(Wait(intro.length));
    }

    void playLoop() {
        source.clip = loop;
        source.loop = true;
        source.Play();
    }

    System.Collections.IEnumerator Wait(float trackLength) {
        yield return new WaitForSecondsRealtime(trackLength);
        playLoop();
    }
}
