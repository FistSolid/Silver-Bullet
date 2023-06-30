using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamCompetitionVersusMode : MonoBehaviour
{
    AsyncOperation operation;


    public IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("ChooseCharMultiGame");
        yield return operation;
    }

    public void Click()
    {
        StartCoroutine(LoadScene());
    }
}
