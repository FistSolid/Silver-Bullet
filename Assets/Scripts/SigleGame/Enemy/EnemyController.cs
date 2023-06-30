using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //伤害，生命值，速度，名字
    public float enemyDamage;
    public float enemyHealth, enemyCurrentHealth;
    public float enemySpeed;
    public string enemyName;
    //最大伤害，最大生命，最大速度
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

    public void SetEnemyAttribute(float damage = 10f, float health = 8f, float speed = 6f, string name = "Zombie")//设置僵尸属性
    {
        enemyName = name;
        SetMaxValue(name);
        if (damage > enemyMaxDamage) { enemyDamage = enemyMaxDamage; } else { enemyDamage = damage; }
        if (health > enemyMaxHealth) { enemyHealth = enemyMaxHealth; } 
            else { enemyHealth = health;  }
        if (speed > enemyMaxSpeed) { enemySpeed = enemyMaxSpeed; } else { enemySpeed = speed; }
    }
    public void SetMaxValue(string name)//设置不同敌人的最大数值
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
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);//生成僵尸
    }



    public void TakeDamage(string id, float damage)
    {

        enemyCurrentHealth -= damage;

        if (enemyCurrentHealth <= 0) 
        {
            AudioManager.instance.PlayBGS("enedeath");//播放死亡音效
            EnemyDeath(id);

        }
    }
    //敌人死亡
    public void EnemyDeath(string id)
    {
        //当后面有多人对战时，可以改为数组
        PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player.KillEnemy(enemyName, id);
        DropItem();//掉落物品
        Destroy(gameObject);
    }
    //掉落物品
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
