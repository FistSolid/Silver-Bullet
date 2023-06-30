using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    public float attackRange = 1f; // 攻击范围
    public float attackCooldown = 1f; // 攻击冷却时间


    public CapsuleCollider2D capsuleCollider;
    private PlayerController playerController;
    public Animator animator; // 动画控制器
    private bool canAttack = false; // 是否在攻击冷却中
    //public Vector2 center; 
    //public Vector2 size;
    void Start()
    {
        attackDamage = 10f;
        Invoke(nameof(ResetCooldown), 3f);//出生三秒后可以攻击
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //  center = capsuleCollider.offset;
        //size = new Vector2(capsuleCollider.size.x - attackRange * 2f, capsuleCollider.size.y - attackRange * 2f);
    }
    
    void AttackHit()
    {
        canAttack = false;
        playerController.OnAttack(attackDamage);// 攻击命中，

        Invoke(nameof(ResetCooldown), 3f);//重置攻击cd
    }
   
    void ResetCooldown()
    {
        canAttack = true; // 攻击冷却时间结束，可以进行下一次攻击
    }

    /*
    void OnDrawGizmosSelected()
    {
        // 绘制攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    */

    void Update()
    {
        if (canAttack)
        {
            // 检测是否有目标在攻击范围内
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    AttackHit();
                    animator.SetTrigger("onAttack"); // 播放攻击动画
                    //Invoke(nameof(AttackHit), 1f); // 等待攻击动画上升阶段结束，再进行攻击命中检测
                    //    Invoke("ResetCooldown", attackCooldown); // 设置攻击冷却时间
                    break;
                }
            }
        }
    }
}
    
