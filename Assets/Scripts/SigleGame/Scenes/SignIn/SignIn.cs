using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.Windows;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

public class SignIn : MonoBehaviour
{
    AsyncOperation operation;

    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;

    string usernameIn;
    string passwordIn;

    private void GetInput()//获取玩家输入的值
    {
        usernameIn = UsernameInput.text;
        passwordIn = PasswordInput.text;

    }


    
    public IEnumerator LoadScene()//加载场景
    {
        operation = SceneManager.LoadSceneAsync("StartScene");
        yield return operation;
    }

    public void Click()
    {
        GetInput();
        
        StartCoroutine(SendRequest(usernameIn, passwordIn));//发送登录请求
        
    }


    IEnumerator SendRequest(string username, string password)
    {

       // string url = "http://101.201.39.2:8080/account/login"; // URL

        Dictionary<string, object> jsonData = new Dictionary<string, object>();
        jsonData.Add("username", username);
        jsonData.Add("password", password);
        string json = JsonConvert.SerializeObject(jsonData);

        UriBuilder uriBuilder = new UriBuilder("http://101.201.39.2:8080/account/login");

        // 创建一个包含查询参数的字典
        Dictionary<string, string> queryParams = new Dictionary<string, string>();
        queryParams.Add("username", username);
        queryParams.Add("password", password);

        // 将查询参数添加到 UriBuilder 中
        string queryString = string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
        uriBuilder.Query = queryString;

        // 获取最终的URL
        string finalUrl = uriBuilder.Uri.ToString();

        Debug.Log(finalUrl);

        // 创建UnityWebRequest对象
        UnityWebRequest webRequest = new(finalUrl, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // 设置请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");
        /*
        // 将请求体转换为byte数组并设置到UnityWebRequest中
        byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(body);*/

        // 发送请求并等待响应
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // 获取响应数据
            string response = webRequest.downloadHandler.text;
            JObject responseJson = JObject.Parse(response);
            Debug.Log(responseJson);
            Debug.Log("Response: " + response);
           // Dictionary<string, string> responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);



            string id = (string)responseJson["id"];

            UserData.id = id;
            UserData.name = username;

            UserData.password = password;

            StartCoroutine(LoadScene());
        }
        else
        {
            MessageManager ms = GameObject.Find("MessageManager").GetComponent<MessageManager>();
            ms.Failed();
            // 请求失败，打印错误信息
            Debug.Log("Error: " + webRequest.error);
        }

        // 请求完成后释放资源
        webRequest.Dispose();
    }

}
