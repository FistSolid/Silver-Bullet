using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startchange : MonoBehaviour
{
    AsyncOperation operation;
    private GameObject bgm;//监测是否存在AudioManager组件
    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.FindWithTag("Audio");
    }

    //异步加载场景 start跳转
    public IEnumerator loadScene()
    {
        operation = SceneManager.LoadSceneAsync("VersusMode");
        yield return operation;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void click()
    {
        if (bgm != null){
            AudioManager.instance.PlayBGS("click");
        }
        StartCoroutine(loadScene());
    }

    
   
}
