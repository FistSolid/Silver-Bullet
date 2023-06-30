using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Victory : MonoBehaviour
{
    AsyncOperation operation;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public IEnumerator loadScene()
    {
        operation = SceneManager.LoadSceneAsync("BillingPageSolo");
        yield return operation;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click()
    {
        StartCoroutine(loadScene());
    }
}
