using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//技能名称：黑洞引爆

/*技能描述：使用该技能后，你可以射出一颗子弹。当子弹碰到第一个敌人时，
它将生成一个黑洞。该黑洞具有强大的引力，可以吸附周围的敌人并造成伤害。
*/

public class BlackHole : MonoBehaviour
{
    public float blackHoleDuration = 5f; // 黑洞持续时间
    public float blackHoleRadius = 10f; // 黑洞半径
    public float attractionForce = 5f; // 吸引力
    public float hitDamage = 5f;



    public float flyTime = 0f;
    public string parentId;
    static public bool hasCollided = true;//是否释放黑洞


    public Vector3 direction = new(1, 0, 0);



    public GameObject blackHolePrefab;


    //黑洞创造
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
