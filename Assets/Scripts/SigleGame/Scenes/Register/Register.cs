
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
using Unity.Collections;
using Newtonsoft.Json.Linq;
using System.Linq;

public class Register : MonoBehaviour
{
    AsyncOperation operation;

    public TMP_InputField UsernameInput;
    public TMP_InputField PasswordInput;
    public TMP_InputField PasswordConfirmInput;
    string usernameIn;
    string passwordIn;
    string passwordConfirmIn;

    private void GetInput()//获取玩家输入的值
    {
        usernameIn = UsernameInput.text;
        passwordIn = PasswordInput.text;
        passwordConfirmIn = PasswordConfirmInput.text;
        
    }


    
    public IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync("SignIn");
        yield return operation;
    }
    public void Click()
    {
        GetInput();

        if (passwordIn.Equals(passwordConfirmIn)) 
        { 
            StartCoroutine(SendRequest(usernameIn, passwordIn));//发送注册请求
        }
    }


    IEnumerator SendRequest(string username, string password)
    {
        /*
        string url = "http://101.201.39.2:8080/account/register"; // URL
        
        Dictionary<string, object> jsonData = new Dictionary<string, object>();
        jsonData.Add("username", username);
        jsonData.Add("password", password);
        string json = JsonConvert.SerializeObject(jsonData);
        //Debug.Log("json: " + json);
        // 创建UnityWebRequest对象
        UnityWebRequest webRequest = new(url, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // 设置请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // 将请求体转换为byte数组并设置到UnityWebRequest中
        byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(body);
        Dictionary<string, object> jsonData = new Dictionary<string, object>();
        jsonData.Add("username", username);
        jsonData.Add("password", password);
        string json = JsonConvert.SerializeObject(jsonData);*/

        UriBuilder uriBuilder = new UriBuilder("http://101.201.39.2:8080/account/register");

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
        UnityWebRequest webRequest = new(finalUrl, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // 发送请求并等待响应
        yield return webRequest.SendWebRequest();
       // Debug.Log(webRequest);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("成功");
            // 获取响应数据
            string response = webRequest.downloadHandler.text;
            //string unescapedJsonString = JsonConvert.DeserializeObject<string>(response);
            //string id = JsonConvert.DeserializeObject<JsonResponse>(unescapedJsonString).id;
            Dictionary<string, string> responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            string id = responseDictionary["id"];
            Debug.Log("id: " + id + "\tname: " + username);
            //这里弹窗：恭喜name注册成功，你的id是：+ id，并返回登陆界面 
            StartCoroutine(LoadScene());
        }
        else
        {
            //这里弹窗：注册失败
        }

        // 请求完成后释放资源
        webRequest.Dispose();
    }

}

    public class JsonResponse
    {
        public string id { get; set; }
        public string jwt { get; set; }
        public string message { get; set; }
    }