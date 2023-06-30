using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    public float attackRange = 1f; // ������Χ
    public float attackCooldown = 1f; // ������ȴʱ��


    public CapsuleCollider2D capsuleCollider;
    private PlayerController playerController;
    public Animator animator; // ����������
    private bool canAttack = false; // �Ƿ��ڹ�����ȴ��
    //public Vector2 center; 
    //public Vector2 size;
    void Start()
    {
        attackDamage = 10f;
        Invoke(nameof(ResetCooldown), 3f);//�����������Թ���
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //  center = capsuleCollider.offset;
        //size = new Vector2(capsuleCollider.size.x - attackRange * 2f, capsuleCollider.size.y - attackRange * 2f);
    }
    
    void AttackHit()
    {
        canAttack = false;
        playerController.OnAttack(attackDamage);// �������У�

        Invoke(nameof(ResetCooldown), 3f);//���ù���cd
    }
   
    void ResetCooldown()
    {
        canAttack = true; // ������ȴʱ����������Խ�����һ�ι���
    }

    /*
    void OnDrawGizmosSelected()
    {
        // ���ƹ�����Χ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    */

    void Update()
    {
        if (canAttack)
        {
            // ����Ƿ���Ŀ���ڹ�����Χ��
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    AttackHit();
                    animator.SetTrigger("onAttack"); // ���Ź�������
                    //Invoke(nameof(AttackHit), 1f); // �ȴ��������������׶ν������ٽ��й������м��
                    //    Invoke("ResetCooldown", attackCooldown); // ���ù�����ȴʱ��
                    break;
                }
            }
        }
    }
}
    
