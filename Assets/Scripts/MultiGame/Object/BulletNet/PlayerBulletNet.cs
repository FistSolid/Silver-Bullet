using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SocialPlatforms;
using Unity.VisualScripting;

public class PlayerBulletNet : NetworkBehaviour
{
    public float flySpeed = 18f;
    public float flyTime = 0f;
    public float hitDamage = 4f;

    public string parentId;

    private Vector3 direction;

    private Rigidbody2D rb;


    private Move playerMove;
    private Quaternion rotation;
    private float angle = 0f;

    public GameObject bulletPrefab;


    void Start()
    {        
        Destroy(gameObject, 10f);
    }

    public void CreatePlayerBulletNet(Vector3 spawnPosition, Vector3 flyDirection, float speed, float damege, float flyAngle, string id)
    {
        parentId = id;
        direction = flyDirection;
        angle = flyAngle;
        flySpeed = speed;
        hitDamage = damege;
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * 18f;
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.gameObject.GetComponent<PlayerControllerNet>().id != parentId)
        {
            PlayerControllerNet player = collision.GetComponent<PlayerControllerNet>();

            player.OnAttack(hitDamage);
            Destroy(gameObject);
        }
    }

}
