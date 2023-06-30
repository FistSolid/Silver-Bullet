using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerInBillingSolo : MonoBehaviour
{
    public TMP_Text username;

    public TMP_Text killAndScore;
    public TMP_Text killZombie;
    public TMP_Text killBat;
    public TMP_Text killWitch;


    private void Start()
    {
        username.text = UserData.name;

        int kill = UserScore.killZombie + UserScore.killWitch + UserScore.killWitch;

        killAndScore.text = "kill: " + kill + "\tscore: " + UserScore.score;

        killZombie.text = UserScore.killZombie.ToString();

        killBat.text = UserScore.killBat.ToString();

        killWitch.text = UserScore.killWitch.ToString();

    }
}

