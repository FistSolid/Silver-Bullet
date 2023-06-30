using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMove : MonoBehaviour
{

    public float detectionRange; // 怪物停止移动并发动攻击的检测范围

    public float moveSpeed; // 怪物的移动速度


    private Transform player; // 玩家的 Transform 组件


    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

    }
    private void Update()
    {
        // 计算怪物与玩家的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);


        if (distanceToPlayer > detectionRange)
        {
            MoveTowardsPlayer();
        }

    }

    private void MoveTowardsPlayer()
    {
        // 根据玩家位置移动怪物
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


}
