using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyOnAttack : MonoBehaviour
{

    public GameObject enemy;
    private EnemyController enemyController;

    public CapsuleCollider2D capsuleCollider2D;
    public ParticleSystem Particle;

    public 
    // Start is called before the first frame update
    void Start()
    {
        enemyController = enemy.GetComponent<EnemyController>();
    }

    public void OnAttack(Collider2D collision)
    {
        if (collision.name.Equals("bullet") || collision.name.Equals("bullet(Clone)"))//碰撞的对象是子弹
        {
            PlayerBullet plyBlt = collision.GetComponent<PlayerBullet>();
            
            enemyController.TakeDamage(plyBlt.parentId, plyBlt.hitDamage);
        }
        else if (collision.name.Equals("fireBall") || collision.name.Equals("fireBall(Clone)"))//碰撞的对象是火球
        {
            FireBall fireball = collision.GetComponent<FireBall>();

            enemyController.TakeDamage(fireball.parentId, fireball.hitDamage);
        }
        else if (collision.name.Equals("A-repel") || collision.name.Equals("A-repel(Clone)"))
        {
            IceThaw iceThaw = collision.GetComponent<IceThaw>();

            enemyController.TakeDamage(iceThaw.parentId, iceThaw.hitDamage);
        }
        else if (collision.name.Equals("B-blackhole") || collision.name.Equals("B-blackhole(Clone)"))
        {
            BlackHole blackHole = collision.GetComponent<BlackHole>();

            enemyController.TakeDamage(blackHole.parentId, blackHole.hitDamage);
        }
        else if (collision.name.Equals("B-Bullet") || collision.name.Equals("B-Bullet(Clone)"))
        {
            PlayerBullet plyBlt = collision.GetComponent<PlayerBullet>();

            enemyController.TakeDamage(plyBlt.parentId, plyBlt.hitDamage);
        }
        else if(!collision.name.Equals("magicBullet")|| collision.name.Equals("magicBullet(Clone)"))
        {
            MagicBullet magicBullet = collision.GetComponent<MagicBullet>();

            enemyController.TakeDamage(magicBullet.parentId, magicBullet.hitDamage);
        }
        //UpdateBloodBar();
        Instantiate(Particle, collision.transform.position, Quaternion.identity);
        //Vector3 attackDirection = player.transform.position - collision.transform.position
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnAttack(collision);
        }
    }

}
