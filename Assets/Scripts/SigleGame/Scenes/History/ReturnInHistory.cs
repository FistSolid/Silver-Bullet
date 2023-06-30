using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnInHistory : MonoBehaviour
{
    AsyncOperation operation;
    private GameObject bgm;//监测是否存在AudioManager组件
    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.FindWithTag("Audio");
    }

    public IEnumerator loadScene()
    {
        operation = SceneManager.LoadSceneAsync("StartScene");
        yield return operation;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void click()
    {
        if (bgm != null)
        {
            AudioManager.instance.PlayBGS("click");
        }
        StartCoroutine(loadScene());
    }
}
