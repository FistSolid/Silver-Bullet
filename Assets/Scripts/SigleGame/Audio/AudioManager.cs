using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource theAS;
    AudioSource[] pauseAudios;
    public Slider volumecontroll;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        theAS = GetComponent<AudioSource>();
    }



    public void PlayBGS(string _audio)
    {
        var audioClip = Resources.Load<AudioClip>("BGS/"+_audio);
        theAS.PlayOneShot(audioClip);

    }

    public void PlayBGM(string _audio)
    {
        var audioClip = Resources.Load<AudioClip>("BGM/" + _audio);
        theAS.PlayOneShot(audioClip);

    }

    public void Continue()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    public IEnumerator Weak()
    {
        while (theAS.volume > 0.02f)
        {
            theAS.volume -= 0.01f;
            yield return new WaitForSeconds(0.02f);
        }

    }

    public IEnumerator WeakBGM(AudioSource bgm)
    {
        while (bgm.volume > 0.02f)
        {
            bgm.volume -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void PauseAllAudio()
    {
        // 获取场景所有音频组件
        pauseAudios = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < pauseAudios.Length; i++)
        {
            pauseAudios[i].Pause();
        }
    }

    public void StopAllAudio()
    {
        AudioSource[] audioSource = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < audioSource.Length; i++)
        {
            Destroy(audioSource[i]);
        }
        GC.Collect();
    }

}
