using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraControllerNet : NetworkBehaviour
{
    public Transform target;
    public float smoothing;

    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            if (transform.position != target.position)
            {
                Vector3 vec = target.position;
                transform.position = Vector3.Lerp(transform.position, vec, smoothing);
            }
        }
    }

}
