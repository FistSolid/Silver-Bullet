using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM("startBGM");
        AudioManager.instance.PlayBGS("start");
        AudioManager.instance.Continue();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
