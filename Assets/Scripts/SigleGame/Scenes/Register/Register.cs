
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

    private void GetInput()//��ȡ��������ֵ
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
            StartCoroutine(SendRequest(usernameIn, passwordIn));//����ע������
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
        // ����UnityWebRequest����
        UnityWebRequest webRequest = new(url, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // ��������ͷ
        webRequest.SetRequestHeader("Content-Type", "application/json");

        // ��������ת��Ϊbyte���鲢���õ�UnityWebRequest��
        byte[] body = new System.Text.UTF8Encoding().GetBytes(json);
        webRequest.uploadHandler = new UploadHandlerRaw(body);
        Dictionary<string, object> jsonData = new Dictionary<string, object>();
        jsonData.Add("username", username);
        jsonData.Add("password", password);
        string json = JsonConvert.SerializeObject(jsonData);*/

        UriBuilder uriBuilder = new UriBuilder("http://101.201.39.2:8080/account/register");

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
        UnityWebRequest webRequest = new(finalUrl, "POST");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // �������󲢵ȴ���Ӧ
        yield return webRequest.SendWebRequest();
       // Debug.Log(webRequest);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("�ɹ�");
            // ��ȡ��Ӧ����
            string response = webRequest.downloadHandler.text;
            //string unescapedJsonString = JsonConvert.DeserializeObject<string>(response);
            //string id = JsonConvert.DeserializeObject<JsonResponse>(unescapedJsonString).id;
            Dictionary<string, string> responseDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

            string id = responseDictionary["id"];
            Debug.Log("id: " + id + "\tname: " + username);
            //���ﵯ������ϲnameע��ɹ������id�ǣ�+ id�������ص�½���� 
            StartCoroutine(LoadScene());
        }
        else
        {
            //���ﵯ����ע��ʧ��
        }

        // ������ɺ��ͷ���Դ
        webRequest.Dispose();
    }

}

    public class JsonResponse
    {
        public string id { get; set; }
        public string jwt { get; set; }
        public string message { get; set; }
    }