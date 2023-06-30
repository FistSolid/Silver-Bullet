using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMove : MonoBehaviour
{

    public float detectionRange; // ����ֹͣ�ƶ������������ļ�ⷶΧ

    public float moveSpeed; // ������ƶ��ٶ�


    private Transform player; // ��ҵ� Transform ���


    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

    }
    private void Update()
    {
        // �����������ҵľ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);


        if (distanceToPlayer > detectionRange)
        {
            MoveTowardsPlayer();
        }

    }

    private void MoveTowardsPlayer()
    {
        // �������λ���ƶ�����
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


}
