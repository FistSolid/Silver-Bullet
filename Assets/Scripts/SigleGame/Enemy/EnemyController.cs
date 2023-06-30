using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //�˺�������ֵ���ٶȣ�����
    public float enemyDamage;
    public float enemyHealth, enemyCurrentHealth;
    public float enemySpeed;
    public string enemyName;
    //����˺����������������ٶ�
    public float enemyMaxDamage;
    public float enemyMaxHealth;
    public float enemyMaxSpeed;


   // public Rigidbody2D rigidbody2;
    public CapsuleCollider2D capsuleCollider2;
    public Vector3 enemyPosition;

    public GameObject enemyPrefab;
    public GameObject hpBottle;
    public GameObject mpBottle;

    private void Start()
    {
        enemyCurrentHealth = enemyHealth;
    }

    public void SetEnemyAttribute(float damage = 10f, float health = 8f, float speed = 6f, string name = "Zombie")//���ý�ʬ����
    {
        enemyName = name;
        SetMaxValue(name);
        if (damage > enemyMaxDamage) { enemyDamage = enemyMaxDamage; } else { enemyDamage = damage; }
        if (health > enemyMaxHealth) { enemyHealth = enemyMaxHealth; } 
            else { enemyHealth = health;  }
        if (speed > enemyMaxSpeed) { enemySpeed = enemyMaxSpeed; } else { enemySpeed = speed; }
    }
    public void SetMaxValue(string name)//���ò�ͬ���˵������ֵ
    {
        if (name.Equals("Zombies"))
        {
            enemyMaxDamage = 20f;
            enemyMaxHealth = 24f;
            enemyMaxSpeed = 8f;

        }
        if (name.Equals("Bat"))
        {
            enemyMaxDamage = 35f;
            enemyMaxHealth = 25f;
            enemyMaxSpeed = 6f;
        }
        if (name.Equals("Witch"))
        {
            enemyMaxDamage = 20f;
            enemyMaxHealth = 30f;
            enemyMaxSpeed = 6f;
        }
    }
    public void CreateEnemy(float damage, float health, float speed, string name, Vector3 spawnPosition)
    {
        SetEnemyAttribute(damage, health, speed, name);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);//���ɽ�ʬ
    }



    public void TakeDamage(string id, float damage)
    {

        enemyCurrentHealth -= damage;

        if (enemyCurrentHealth <= 0) 
        {
            AudioManager.instance.PlayBGS("enedeath");//����������Ч
            EnemyDeath(id);

        }
    }
    //��������
    public void EnemyDeath(string id)
    {
        //�������ж��˶�սʱ�����Ը�Ϊ����
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.KillEnemy(enemyName, id);
        DropItem();//������Ʒ
        Destroy(gameObject);
    }
    //������Ʒ
    public void DropItem()
    {
        float dropRate = 0.1f;
        if (Random.value <= dropRate)
        {
            Instantiate(hpBottle, transform.position, Quaternion.identity);
        }
        if (Random.value <= dropRate)
        {
            Instantiate(mpBottle, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
