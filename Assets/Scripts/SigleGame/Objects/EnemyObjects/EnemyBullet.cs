using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

public class EnemyBullet : MonoBehaviour
{
    public float flySpeed = 3f;
    public float flyTime = 0f;
    public float bulletDamage = 10f;
    public Vector3 direction;

    public Rigidbody2D rb;

    public PlayerController player;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void CreateEnemyBullet(Vector3 spawnPosition,Vector3 flyDirection, float speed, float damege)
    {
        Instantiate(gameObject, spawnPosition, Quaternion.identity); // ʵ�����ӵ�����
        flySpeed = speed;
        bulletDamage = damege;
        direction = flyDirection; //���򣺴ӵ���ָ���ӵ�λ��
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))//�����ײ����
        {
            player.OnAttack(bulletDamage);
            Destroy(gameObject);
        }
        
    }


    void Update()
    { 
        //�ӵ�����
        rb.transform.position += flySpeed * Time.deltaTime * direction;
        flyTime += Time.deltaTime;

        if (flyTime > 15f)
        {
            Destroy(gameObject);
        }

        //�ӵ���ײ


    }
}
