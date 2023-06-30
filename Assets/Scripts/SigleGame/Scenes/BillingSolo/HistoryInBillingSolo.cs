using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HistoryInBillingSolo : MonoBehaviour
{
    AsyncOperation operation;
    public string id;
    public string username;
    public string password;

    private void Start()
    {
        id = UserData.id;
        username = UserData.name;
        password = UserData.password;
    }

    public void Click()
    {
        //AudioManager.instance.PlayBGS("click");//点击按钮的音效，如果没有从start/mainsene页面开始游戏测试，这行代码会导致按钮无法跳转
        StartCoroutine(SendRequest(id, username, password));
        

    }

    private IEnumerator LoadScene()
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
    


}
