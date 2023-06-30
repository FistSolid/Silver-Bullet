using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoloGO : MonoBehaviour
{
    AsyncOperation operation;
    public Animator fade;
    public GameObject bgm;
    // Start is called before the first frame update
    void Start()
    {
       fade = GameObject.Find("TransToMain").GetComponent<Animator>();
       bgm = GameObject.Find("StartAudio");
    }
    public IEnumerator loadScene()
    {
        fade.SetTrigger("out");//播放转场特效
        yield return new WaitForSeconds(2f);//延迟两秒后跳转
        if (bgm != null)
        {
            Destroy(bgm);
        }
        SceneManager.LoadSceneAsync("MainScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click()
    {
        if (bgm != null)
        {
            AudioManager.instance.PlayBGS("flash");
        }

        StartCoroutine(loadScene());
        
    }
}
