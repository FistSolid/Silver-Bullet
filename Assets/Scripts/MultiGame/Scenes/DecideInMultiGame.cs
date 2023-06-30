using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecideInMultiGame : MonoBehaviour
{
    AsyncOperation operation;


    public IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("MultiMainScene");
        yield return operation;
    }

    public void Click()
    {
        StartCoroutine(LoadScene());
    }
}
