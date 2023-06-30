using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButtonInGameOverMultiGame : MonoBehaviour
{
    AsyncOperation operation;

    
    public IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("StartScene");
        yield return operation;
    }

    public void Click()
    {
        StartCoroutine(LoadScene());
    }
}
