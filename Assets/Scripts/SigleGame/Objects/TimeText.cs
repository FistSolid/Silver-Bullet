using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    public Text CountDown;
    public float TimeRemain = 60f;
    // Start is called before the first frame update
    void Start()
    {
       CountDown = gameObject.GetComponent<Text>();
       CountDown.text = TimeRemain.ToString("F2");
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeRemain >= 0f)
        {
            TimeRemain -= Time.deltaTime;
            CountDown.text = TimeRemain.ToString("F2");
        }
        
    }
}
