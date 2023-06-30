using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UIManagerInMainScene : MonoBehaviour
{
    private string character;

    public Sprite[] headArray;
    public Sprite[] skillArray;
    public Image head;
    public Image skill1;
    public Image skill2;

    void Start()
    {
        character = UserData.character;
        if (character.Equals("Angelica"))
        {

            head.sprite = headArray[0];
            skill1.sprite = skillArray[0];
            skill2.sprite = skillArray[1];

        }
        else if (character.Equals("Belles"))
        {
            head.sprite = headArray[1];
            skill1.sprite = skillArray[2];
            skill2.sprite = skillArray[3];

        }
    }
}
