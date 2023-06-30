using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemy : MonoBehaviour
{
    // �����������ӱ���
    public float enemyCountMultiplier = 1.006f;
    //�����������
    private int enemyMaxNum = 50;
    //���˲���
    public int enemyWave = 0;
    //��������
    public int enemyNum = 15;
    //����ʣ��
    public int enemyRemain;
    /*
    //������˵�����
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
        for (int i = 0; i < SpawnEnemiesForWave(enemyWave); i++)//�м��ѭ�����������޸�
        {
            float randomX = Random.Range(maxPosition.x, minPosition.x);
            float randomY = Random.Range(maxPosition.y, minPosition.y);

            Vector3 spawnPosition = new (randomX, randomY, 0f); //���ɵ����λ��

                //α����������ֵ���
                float[] enemyRatios = { 0.4f, 0.4f, 0.2f }; // �������ɱ���
                float rand = Random.Range(0f, 1f); // ����0��1֮��������
                float sum = 0f; // �ۼӱ���

                for (int j = 0; j < enemyRatios.Length; j++)
                {
                    sum += enemyRatios[j];
                    if (sum > rand)
                    {
                    // ���ɵ� i �ֵ���
                    SpawnAkindOfEnemy(j, spawnPosition, enemyWave);
                        break;
                    }
                }
        }
    }

    private void SpawnAkindOfEnemy(int index, Vector3 spawnPosition,int enemyWave) 
    {
        if(index == 0) //����Zombie
        {
            EnemyController enemyController = enemyZombie.GetComponent<EnemyController>();
            float enemyHealth = StrengthenEnemyHealth(index, enemyWave);
            float enemyDamage = StrengthenEnemyDamage(index, enemyWave);
            enemyController.SetEnemyAttribute(enemyHealth, enemyDamage, 6f, "Zombie");
            enemyController.CreateEnemy(enemyDamage, enemyHealth, 6f, "Zombie", spawnPosition);
        }
        else if(index == 1) //����Bat
        {
            EnemyController enemyController = enemyBat.GetComponent<EnemyController>();
            float enemyHealth = StrengthenEnemyHealth(index, enemyWave);
            float enemyDamage = StrengthenEnemyDamage(index, enemyWave);
            enemyController.SetEnemyAttribute(enemyHealth, enemyDamage, 6f, "Bat");
            enemyController.CreateEnemy(enemyDamage, enemyHealth, 6f, "Bat", spawnPosition);
        }
        else //����Witch
        {
            EnemyController enemyController = enemyWitch.GetComponent<EnemyController>();
            float enemyHealth = StrengthenEnemyHealth(index, enemyWave);
            float enemyDamage = StrengthenEnemyDamage(index, enemyWave);
            enemyController.SetEnemyAttribute(enemyHealth, enemyDamage, 6f, "Witch");
            enemyController.CreateEnemy(enemyDamage, enemyHealth, 6f, "Witch", spawnPosition);
        }

    }

    //��ǿ����ֵ
    private float StrengthenEnemyHealth(int index, int enemyWave)
    {
        if (index == 0) //��ǿZombie
        {
            
            float enemyHealth = 8f + 2 * enemyWave;
            return enemyHealth;
        }
        else if (index == 1) //��ǿBat
        {
            
            float enemyHealth = 14f + 2 * enemyWave;
            return enemyHealth;
            
        }
        else //��ǿWitch
        {
           
            float enemyHealth = 18f + 2 * enemyWave;
            return enemyHealth;
            
        }
    }

    
    //��ǿ�˺�
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

    //�沨���������ӵ�������
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // ʹ�� "Enemy" ��ǩ�����ҵ�����Ϸ����
        if (enemies.Length == 0 || timer >= 60f)
        {
            timer = 0f;

            //������Լ��뵹��ʱ
            SpawnWave();
            // ����û��ʣ�����
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
