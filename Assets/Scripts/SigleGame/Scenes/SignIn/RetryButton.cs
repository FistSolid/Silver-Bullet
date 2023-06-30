using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RetryButton : MonoBehaviour
{
    public void Click()
    {
        MessageManager ms =GameObject.Find("MessageManager").GetComponent<MessageManager>();
        ms.Close();
    }
}
