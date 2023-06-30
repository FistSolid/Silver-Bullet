using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpawnPlayer : MonoBehaviour
{
    private string character;

    public GameObject AngelicaPrefab;
    public GameObject BellesPrefab;


    void Start()
    {
        character = UserData.character;
        if (character.Equals("Angelica"))
        {
            Instantiate(AngelicaPrefab, new(0, 0, 0), Quaternion.identity);//Éú³ÉÍæ¼Ò
        }

        else if (character.Equals("Belles"))
        {
            Instantiate(BellesPrefab, new(0,0,0), Quaternion.identity);
        }
    }

}
