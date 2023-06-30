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

        // ����һ��������ѯ�������ֵ�
        Dictionary<string, string> queryParams = new Dictionary<string, string>();
        queryParams.Add("id", id);
        queryParams.Add("username", username);
        queryParams.Add("password", password);

        // ����ѯ������ӵ� UriBuilder ��
        string queryString = string.Join("&", queryParams.Select(kvp => kvp.Key + "=" + kvp.Value));
        uriBuilder.Query = queryString;

        // ��ȡ���յ�URL
        string finalUrl = uriBuilder.Uri.ToString();
        Debug.Log("url" + finalUrl);
        // ����UnityWebRequest����
        UnityWebRequest webRequest = new(finalUrl, "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        // ��������ͷ
        webRequest.SetRequestHeader("Content-Type", "application/json");
        // �������󲢵ȴ���Ӧ
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest);
        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // ��ȡ��Ӧ����
            string response = webRequest.downloadHandler.text;

            Debug.Log("response" + response);
            UserScore.historyScore = response;
             StartCoroutine(LoadScene());
        }
        else
        {
            // ����ʧ�ܣ���ӡ������Ϣ
            Debug.Log("Error: " + webRequest.error);
        }

        // ������ɺ��ͷ���Դ
        webRequest.Dispose();
    }


    public void Click()
    {
        StartCoroutine(SendRequest(UserData.id, UserData.name, UserData.password));
        AudioManager.instance.PlayBGS("click");

    }
}
