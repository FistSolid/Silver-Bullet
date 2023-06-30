using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnInRegister : MonoBehaviour
{
    AsyncOperation operation;

    public void Click()
    {
       // AudioManager.instance.PlayBGS("click");
        StartCoroutine(LoadScene());
    }
    public IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("SignIn");
        yield return operation;
    }
}
