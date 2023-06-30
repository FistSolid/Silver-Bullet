using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAttack : MonoBehaviour
{
    public GameObject WitchBulletPrefab;
    private EnemyBullet eneBlt;

    public int numBullets = 8; // �ӵ�����
    public float radius = 1.0f; // �ӵ����ɵİ뾶

    public bool CanAttack = false;

    void Start()
    {   
        eneBlt = WitchBulletPrefab.GetComponent<EnemyBullet>();
        Invoke(nameof(ResetCanAttack), Random.Range(2f, 6f));
    }

    public void SpawnBullet()
    {
        float angleStep = 360.0f / numBullets; // ����ÿ���ӵ�֮��ĽǶȼ��

        for (int i = 0; i < numBullets; i++)
        {
            float angle = i * angleStep; // ���㵱ǰ�ӵ��ĽǶ�λ��
            Vector3 spawnPosition = transform.position + Quaternion.Euler(0, 0, angle) * Vector3.right * radius; // ���ݽǶȺͰ뾶��������λ��

            eneBlt.CreateEnemyBullet(spawnPosition, (spawnPosition - transform.position).normalized, 3f, 10f);
        }
        CanAttack = false;
        Invoke(nameof(ResetCanAttack), 5f);
    }
    public void ResetCanAttack()
    {
        CanAttack = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (CanAttack) 
        {
            SpawnBullet();
        }
    }
}
