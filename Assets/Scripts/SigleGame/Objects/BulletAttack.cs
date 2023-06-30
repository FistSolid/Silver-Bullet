using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    public BlackHole blackHole;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!BlackHole.hasCollided && collision.gameObject.CompareTag("Enemy"))
        {
            BlackHole.hasCollided = true;

            // 获取碰撞点

            Vector3 collisionPoint = collision.bounds.ClosestPoint(this.transform.position);

            // 生成黑洞
            blackHole.CreateBlackHole(collisionPoint, 10f, playerController.id);

            // 销毁子弹
            Destroy(gameObject);

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
