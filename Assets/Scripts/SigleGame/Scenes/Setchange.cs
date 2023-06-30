using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setchange : MonoBehaviour
{
    AsyncOperation operation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator loadScene()
    {
        operation = SceneManager.LoadSceneAsync("SetChange");
        yield return operation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void click()
    {
        AudioManager.instance.PlayBGS("click");
        StartCoroutine(loadScene());
    }

}
