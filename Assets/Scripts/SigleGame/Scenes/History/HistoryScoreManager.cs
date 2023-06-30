using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System;

public class HistoryScoreManager : MonoBehaviour
{
    public Image[] headImage;
    public TMP_Text[] data;

    public Sprite[] characterPhoto;

    private string character;
    private string kill;
    private string grade;
    private string time;

    private void Start()
    {

        JObject json = JObject.Parse(UserScore.historyScore);
        Debug.Log(json);
        JArray scoreArray = (JArray)json["SixGrades"];
        Debug.Log(scoreArray);
        for (int i = 0; i < scoreArray.Count; i++)
        {
            character = (string)scoreArray[i]["role"];
            Debug.Log("character" +  character);
            kill = (string)scoreArray[i]["kill"];
            grade = (string)scoreArray[i]["grade"];
            time = (string)scoreArray[i]["time"];
            /*
            DateTime dateTime = DateTime.ParseExact(time[i], "yyyy-MM-dd'T'HH:mm:ss.fffffffzzz", null);
            string formattedDateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");*/

            if (character.Equals("Angelica"))
            {
                headImage[i].sprite = characterPhoto[0];
            }
            else
            {
                headImage[i].sprite = characterPhoto[1];
            }

            data[i].text = "kill: " + kill + "\tscore: " + grade + "\ttime: " + time; 
        }
        

    }
}
