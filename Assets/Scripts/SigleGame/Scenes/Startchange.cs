using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startchange : MonoBehaviour
{
    AsyncOperation operation;
    private GameObject bgm;//����Ƿ����AudioManager���
    // Start is called before the first frame update
    void Start()
    {
        bgm = GameObject.FindWithTag("Audio");
    }

    //�첽���س��� start��ת
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
