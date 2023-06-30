using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    private void Start()
    {
       target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            if(transform.position != target.position) 
            {
                Vector3 vec = target.position;
                transform.position = Vector3.Lerp(transform.position, vec, smoothing);
            }
        }
    }
    void Update()
    {
        
    }
}
