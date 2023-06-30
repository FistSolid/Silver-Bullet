using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacter: MonoBehaviour
{
    public Button buttonAngelica;
    public Button buttonBelles;

    private void Start()
    {
        buttonAngelica.onClick.AddListener(OnButtonAngelicaClicked);
        buttonBelles.onClick.AddListener(OnButtonBellesClicked);
    }

    private void OnButtonAngelicaClicked()
    {
        UserData.character = "Angelica";
    }

    private void OnButtonBellesClicked()
    {
        UserData.character = "Belles";
    }
}
