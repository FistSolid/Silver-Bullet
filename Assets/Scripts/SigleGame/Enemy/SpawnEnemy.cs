using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemy : MonoBehaviour
{
    // 敌人数量增加倍数
    public float enemyCountMultiplier = 1.006f;
    //敌人最大数量
    private int enemyMaxNum = 50;
    //敌人波次
    public int enemyWave = 0;
    //敌人数量
    public int enemyNum = 15;
    //敌人剩余
    public int enemyRemain;
    /*
    //各类敌人的数量
    public int enemyZobiesNum;
    public int enemyBatNum;
    public int enemyWitchNum;
    */
    public float timer = 0f;


    public GameObject enemyZombie;
    public GameObject enemyBat;
    public GameObject enemyWitch;

    public Text waveText;

    public Vector3 maxPosition;
    public Vector3 minPosition;

    void Start()
    {
        enemyRemain = enemyNum;
    }
    private void SpawnWave()
    {
        enemyWave++;
        waveText.text = "Wave " + enemyWave; 
        for (int i = 0; i < SpawnEnemiesForWave(enemyWave); i++)//中间的循环条件仍需修改
        {
            float randomX = Random.Range(maxPosition.x, minPosition.x);
            float randomY = Random.Range(maxPosition.y, minPosition.y);

            Vector3 spawnPosition = new (randomX, randomY, 0f); //生成的随机位置

                //伪随机生成三种敌人
                float[] enemyRatios = { 0.4f, 0.4f, 0.2f }; // 敌人生成比例
                float rand = Random.Range(0f, 1f); // 生成0到1之间的随机数
                float sum = 0f; // 累加变量

                for (int j = 0; j < enemyRatios.Length; j++)
                {
                    sum += enemyRatios[j];
                    if (sum > rand)
                    {
                    // 生成第 i 种敌人
                    SpawnAkindOfEnemy(j, spawnPosition, enemyWave);
                        break;
                    }
                }
        }
    }

    private void SpawnAkindOfEnemy(int index, Vector3 spawnPosition,int enemyWave) 
    {
        if(index == 0) //生成Zombie
        {
            EnemyController enemyController = enemyZombie.GetComponent<EnemyController>();
            float enemyHealth = StrengthenEnemyHealth(index, enemyWave);
            float enemyDamage = StrengthenEnemyDamage(index, enemyWave);
            enemyController.SetEnemyAttribute(enemyHealth, enemyDamage, 6f, "Zombie");
            enemyController.CreateEnemy(enemyDamage, enemyHealth, 6f, "Zombie", spawnPosition);
        }
        else if(index == 1) //生成Bat
        {
            EnemyController enemyController = enemyBat.GetComponent<EnemyController>();
            float enemyHealth = StrengthenEnemyHealth(index, enemyWave);
            float enemyDamage = StrengthenEnemyDamage(index, enemyWave);
            enemyController.SetEnemyAttribute(enemyHealth, enemyDamage, 6f, "Bat");
            enemyController.CreateEnemy(enemyDamage, enemyHealth, 6f, "Bat", spawnPosition);
        }
        else //生成Witch
        {
            EnemyController enemyController = enemyWitch.GetComponent<EnemyController>();
            float enemyHealth = StrengthenEnemyHealth(index, enemyWave);
            float enemyDamage = StrengthenEnemyDamage(index, enemyWave);
            enemyController.SetEnemyAttribute(enemyHealth, enemyDamage, 6f, "Witch");
            enemyController.CreateEnemy(enemyDamage, enemyHealth, 6f, "Witch", spawnPosition);
        }

    }

    //加强生命值
    private float StrengthenEnemyHealth(int index, int enemyWave)
    {
        if (index == 0) //加强Zombie
        {
            
            float enemyHealth = 8f + 2 * enemyWave;
            return enemyHealth;
        }
        else if (index == 1) //加强Bat
        {
            
            float enemyHealth = 14f + 2 * enemyWave;
            return enemyHealth;
            
        }
        else //加强Witch
        {
           
            float enemyHealth = 18f + 2 * enemyWave;
            return enemyHealth;
            
        }
    }

    
    //加强伤害
    private float StrengthenEnemyDamage(int index, int enemyWave)
    {
        if (index == 0)
        {
            float enemyDamage = 10f + 2 * enemyWave;
            return (enemyDamage);
        }
       else if (index == 1)
        {
            float enemyDamage = 15f+ 2 * enemyWave;
            return (enemyDamage);
        }
        else
        {
            float enemyDamage = 10f + 2 * enemyWave;
           return(enemyDamage);
        }
    }

    //随波次增加增加敌人数量
    private int SpawnEnemiesForWave(int waveNumber)
    {
        if (waveNumber <= 5)
        {
            if (enemyNum <= enemyMaxNum)
            {
                enemyNum = (int)Mathf.Sqrt(enemyNum + waveNumber * 5);
                return enemyNum;
            }
            else
            {
                return enemyNum;
            }
        }
        else if (waveNumber > 5 && waveNumber <= 10)
        {
            if (enemyNum <= enemyMaxNum)
            {
                enemyNum = 2 * (int)Mathf.Sqrt(enemyNum + waveNumber * 5);
                return enemyNum;
            }
            else
            {
                return enemyNum;
            }

        }
        else 
        {
            if (enemyNum <= enemyMaxNum)
            {
                enemyNum = 4 * (int)Mathf.Sqrt(enemyNum + waveNumber * 5);
                return enemyNum;
            }
            else
            {
                return enemyNum;
            }
        }
    }
    private void CheckRemainingEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // 使用 "Enemy" 标签来查找敌人游戏对象
        if (enemies.Length == 0 || timer >= 60f)
        {
            timer = 0f;

            //这里可以加入倒计时
            SpawnWave();
            // 场上没有剩余敌人
          // Invoke(nameof(SpawnWave), 3f);
        }
    }



    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        CheckRemainingEnemies();
    }
}
