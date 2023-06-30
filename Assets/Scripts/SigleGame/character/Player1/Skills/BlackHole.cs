using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������ƣ��ڶ�����

/*����������ʹ�øü��ܺ���������һ���ӵ������ӵ�������һ������ʱ��
��������һ���ڶ����úڶ�����ǿ�������������������Χ�ĵ��˲�����˺���
*/

public class BlackHole : MonoBehaviour
{
    public float blackHoleDuration = 5f; // �ڶ�����ʱ��
    public float blackHoleRadius = 10f; // �ڶ��뾶
    public float attractionForce = 5f; // ������
    public float hitDamage = 5f;



    public float flyTime = 0f;
    public string parentId;
    static public bool hasCollided = true;//�Ƿ��ͷźڶ�


    public Vector3 direction = new(1, 0, 0);



    public GameObject blackHolePrefab;


    //�ڶ�����
    public void CreateBlackHole(Vector3 spawnPosition, float damege, string id)
    {
        parentId = id;
        hitDamage = damege;

        Instantiate(blackHolePrefab, spawnPosition, Quaternion.identity);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

         if (collision.CompareTag("Enemy"))
         {
            EnemyController enemyController = collision.GetComponent<EnemyController>();
            enemyController.TakeDamage(parentId, hitDamage);
         }
    }

    private void Update()
    {

        flyTime += Time.deltaTime;

        if (flyTime > 6f)
        {
            Destroy(gameObject);
        }
    
    }
}
