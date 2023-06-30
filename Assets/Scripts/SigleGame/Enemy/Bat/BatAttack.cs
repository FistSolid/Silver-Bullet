using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAttack : MonoBehaviour
{

    private bool canAttack = false; // 标记怪物是否可以攻击

    public GameObject BatBulletPrefab;
    private EnemyBullet eneBlt;
    private GameObject player;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        eneBlt = BatBulletPrefab.GetComponent<EnemyBullet>();
        player = GameObject.FindWithTag("Player");

        Invoke(nameof(ResetCanAttack), Random.Range(1f,4f));
    }

    

    private void Attack()
    {
        
        canAttack = false;
        animator.SetTrigger("onAttack");

        Vector3 direction = (player.transform.position - transform.position).normalized;
        eneBlt.CreateEnemyBullet(transform.position, direction, 10f, 15f);

        Invoke(nameof(ResetCanAttack), 3.5f);
    }       
    private void ResetCanAttack()
    {
       canAttack = true;
    }

    private void Update()
    {
        if (canAttack) 
        {
            Attack();
        }
    }
}
