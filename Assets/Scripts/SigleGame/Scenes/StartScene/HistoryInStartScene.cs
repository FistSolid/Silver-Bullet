using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HistoryInStartScene : MonoBehaviour
{
    AsyncOperation operation;
    
    IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("History");
        yield return operation;
    }
    IEnumerator SendRequest(string id, string username, string password)
    {
        UriBuilder uriBuilder = new UriBuilder("http://101.201.39.2:9090/single/get_history_results");

        // 创建一个包含查询参数的字典
        Dictionary<string, string> queryParams = new Dictionary<string, string>();
        queryParams.Add("id", id);
        queryParams.Add("username", username);
        queryParams.Add("password", password);

        // 将查询参数添加到 UriBuilder 中
        string queryString = string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
        uriBuilder.Query = queryString;

        // 获取最终的URL
        string finalUrl = uriBuilder.Uri.ToString();
        Debug.Log("url" + finalUrl);
        // 创建UnityWebRequest对象
        UnityWebRequest webRequest = new(finalUrl, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // 设置请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");
        // 发送请求并等待响应
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // 获取响应数据
            string response = webRequest.downloadHandler.text;

            Debug.Log("response" + response);
            UserScore.historyScore = response;
             StartCoroutine(LoadScene());
        }
        else
        {
            // 请求失败，打印错误信息
            Debug.Log("Error: " + webRequest.error);
        }

        // 请求完成后释放资源
        webRequest.Dispose();
    }


    public void Click()
    {
        StartCoroutine(SendRequest(UserData.id, UserData.name, UserData.password));
        AudioManager.instance.PlayBGS("click");

    }
}
