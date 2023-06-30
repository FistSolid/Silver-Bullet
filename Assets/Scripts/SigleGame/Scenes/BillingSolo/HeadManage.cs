using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadManage : MonoBehaviour
{
    private string character;

    public Sprite[] headArray;
    public Image head;
    // Start is called before the first frame update
    void Start()
    {
        character = UserData.character;
        if (character.Equals("Angelica"))
        {

            head.sprite = headArray[0];
            
        }
        else if (character.Equals("Belles"))
        {
            head.sprite = headArray[1];
            
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
