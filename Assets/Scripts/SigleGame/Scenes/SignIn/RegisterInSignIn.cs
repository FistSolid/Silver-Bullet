using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterInSignIn : MonoBehaviour
{
    AsyncOperation operation;
    public IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("Register");
        yield return operation;
    }
    public void Click()
    {
        StartCoroutine(LoadScene());

    }
}
