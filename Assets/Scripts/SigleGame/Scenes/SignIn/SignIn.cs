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

    private void GetInput()//��ȡ��������ֵ
    {
        usernameIn = UsernameInput.text;
        passwordIn = PasswordInput.text;

    }


    
    public IEnumerator LoadScene()//���س���
    {
        operation = SceneManager.LoadSceneAsync("StartScene");
        yield return operation;
    }

    public void Click()
    {
        GetInput();
        
        StartCoroutine(SendRequest(usernameIn, passwordIn));//���͵�¼����
        
    }


    IEnumerator SendRequest(string username, string password)
    {

       // string url = "http://101.201.39.2:8080/account/login"; // URL

        Dictionary<string, object> jsonData = new Dictionary<string, object>();
        jsonData.Add("username", username);
        jsonData.Add("password", password);
        string json = JsonConvert.SerializeObject(jsonData);

        UriBuilder uriBuilder = new UriBuilder("http://101.201.39.2:8080/account/login");

        // ����һ��������ѯ�������ֵ�
        Dictionary<string, string> queryParams = new Dictionary<string, string>();
        queryParams.Add("username", username);
        queryParams.Add("password", password);

        // ����ѯ������ӵ� UriBuilder ��
        string queryString = string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
        uriBuilder.Query = queryString;

        // ��ȡ���յ�URL
        string finalUrl = uriBuilder.Uri.ToString();

        Debug.Log(finalUrl);

        // ����UnityWebRequest����
        UnityWebRequest webRequest = new(finalUrl, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // ��������ͷ
        webRequest.SetRequestHeader("Content-Type", "application/json");
        /*
        // ��������ת��Ϊbyte���鲢���õ�UnityWebRequest��
        byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(body);*/

        // �������󲢵ȴ���Ӧ
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // ��ȡ��Ӧ����
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
            // ����ʧ�ܣ���ӡ������Ϣ
            Debug.Log("Error: " + webRequest.error);
        }

        // ������ɺ��ͷ���Դ
        webRequest.Dispose();
    }

}
