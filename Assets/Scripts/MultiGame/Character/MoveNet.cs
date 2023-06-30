using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Mirror;
using UnityEngine;




public class MoveNet : NetworkBehaviour
{
    public float moveSpeed = 6f;
    public Vector3 currentDirection = new(1, 0, 0);
    public Vector3 direction;
    public int faceDir = 1;//角色朝向

    private Animator animator;

    private PlayerControllerNet playerController;

    private void Start()
    {
        animator = GetComponent<Animator>();

        playerController = gameObject.GetComponent<PlayerControllerNet>();
    }


    void Update()
    {
        if (isLocalPlayer)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");  // 获取水平输入
            float vertical = Input.GetAxisRaw("Vertical");  // 获取垂直输入
            direction = new(horizontal, vertical, 0);
            Vector3 dir = new(horizontal, vertical, 0);

            //行走
            if (dir != Vector3.zero)
            {

                //设置状态为奔跑
                animator.SetBool("isRun", true);
                transform.localPosition += moveSpeed * Time.deltaTime * direction.normalized;
                playerController.ChangePosition(transform.localPosition);

                if (dir.x >= 0)//角色朝向右边
                {
                    faceDir = 1;
                    transform.localScale = new(faceDir, 1, 1);
                }
                else if (dir.x < 0)//角色朝向左边
                {
                    faceDir = -1;
                    transform.localScale = new(faceDir, 1, 1);
                }

            }
            else
            {
                animator.SetBool("isRun", false);
            }
            //站立
            if (direction.normalized != Vector3.zero)
            {
                currentDirection = direction.normalized;
            }

        }
    }
}
