using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public GameObject messageCanvas;

    public void Failed()
    {
        messageCanvas.SetActive(true);
    }

    public void Close()
    {
        messageCanvas.SetActive(false);
    }
}
