using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerConstrain : MonoBehaviour
{
    public Vector3 maxPosition;
    public Vector3 minPosition;

    public float smoothing;

    private void LateUpdate()
    {
        Vector3 targetPos = transform.position;
        targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);//������ҵ�x����
        targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);//������ҵ�y����

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

}
