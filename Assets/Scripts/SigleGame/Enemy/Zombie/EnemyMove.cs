using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //public GameObject player;
    public PlayerController player;

    public Vector3 playerPosition;
    public Vector3 moveDirection;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.playerPosition;//���λ��
        moveDirection = (playerPosition - transform.position).normalized;//enemy�ƶ�����

        transform.localPosition += moveSpeed * Time.deltaTime * moveDirection;
    }
}
